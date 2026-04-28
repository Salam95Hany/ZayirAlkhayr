using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Inventory;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Inventory
{
    public class ItemRecipeService : IItemRecipeService
    {
        private static readonly Error InvalidRecipeData = new Error("بيانات الوصفة غير صحيحة");
        private static readonly Error InvalidRecipeQuantity = new Error("يجب أن تكون الكمية المطلوبة أكبر من صفر");
        private static readonly Error DuplicateRecipe = new Error("لا يمكن تكرار نفس عنصر المخزون داخل وصفة الصنف");
        private static readonly Error RecipeLinesRequired = new Error("يجب إضافة مكون واحد على الأقل للوصفة");
        private readonly IUnitOfWork _unitOfWork;

        public ItemRecipeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<List<ItemRecipeDetailsDto>>> GetAllItemRecipes(PagingFilterModel model)
        {
            var searchText = GetFilterValue(model, "SearchText");
            var itemId = GetFilterIntValue(model, "ItemId");
            var inventoryItemId = GetFilterIntValue(model, "InventoryItemId");

            var linesQuery = BuildItemRecipeLinesQuery();

            if (!string.IsNullOrWhiteSpace(searchText))
                linesQuery = linesQuery.Where(i => i.ItemName.Contains(searchText) || i.InventoryItemName.Contains(searchText));

            if (itemId.HasValue)
                linesQuery = linesQuery.Where(i => i.ItemId == itemId.Value);

            if (inventoryItemId.HasValue)
                linesQuery = linesQuery.Where(i => i.InventoryItemId == inventoryItemId.Value);

            var lines = await linesQuery
                .OrderBy(i => i.ItemName)
                .ThenBy(i => i.InventoryItemName)
                .ToListAsync();

            var groupedResults = lines
                .GroupBy(i => new { i.ItemId, i.ItemName })
                .Select(group => new ItemRecipeDetailsDto
                {
                    ItemId = group.Key.ItemId,
                    ItemName = group.Key.ItemName,
                    RecipesCount = group.Count(),
                    IngredientsSummary = string.Join("، ", group.Select(i => i.InventoryItemName).Distinct()),
                    InsertUser = group.Select(i => i.InsertUser).FirstOrDefault(i => !string.IsNullOrWhiteSpace(i)),
                    InsertDate = group.Min(i => i.InsertDate),
                    UpdateUser = group.OrderByDescending(i => i.UpdateDate ?? i.InsertDate).Select(i => i.UpdateUser).FirstOrDefault(i => !string.IsNullOrWhiteSpace(i)),
                    UpdateDate = group.Max(i => i.UpdateDate ?? i.InsertDate),
                    Recipes = group
                        .OrderBy(i => i.InventoryItemName)
                        .Select(i => new ItemRecipeLineDetailsDto
                        {
                            ItemRecipeId = i.ItemRecipeId,
                            InventoryItemId = i.InventoryItemId,
                            InventoryItemName = i.InventoryItemName,
                            InventoryItemUnitName = i.InventoryItemUnitName,
                            QuantityNeeded = i.QuantityNeeded
                        }).ToList()
                }).OrderBy(i => i.ItemName).ToList();

            var totalCount = groupedResults.Count;
            var results = ApplyPaging(groupedResults, model).ToList();

            return ApiResponseModel<List<ItemRecipeDetailsDto>>.Success(GenericErrors.GetSuccess, results, totalCount);
        }

        public async Task<ApiResponseModel<ItemRecipeDetailsDto>> GetItemRecipeById(int itemId)
        {
            var lines = await BuildItemRecipeLinesQuery()
                .Where(i => i.ItemId == itemId)
                .OrderBy(i => i.InventoryItemName)
                .ToListAsync();

            if (lines.Count == 0)
                return ApiResponseModel<ItemRecipeDetailsDto>.Failure(GenericErrors.NotFound);

            var data = BuildRecipeGroup(lines);
            return ApiResponseModel<ItemRecipeDetailsDto>.Success(GenericErrors.GetSuccess, data);
        }

        public async Task<ApiResponseModel<string>> AddItemRecipe(ItemRecipeUpsertDto model)
        {
            var validationError = await ValidateRecipeModelAsync(model, false);
            if (validationError != null)
                return ApiResponseModel<string>.Failure(validationError);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var recipeLine in model.Recipes)
                {
                    await _unitOfWork.Repository<ItemRecipe>().AddAsync(new ItemRecipe
                    {
                        ItemId = model.ItemId,
                        InventoryItemId = recipeLine.InventoryItemId,
                        QuantityNeeded = recipeLine.QuantityNeeded,
                        InsertUser = model.UserId,
                        InsertDate = DateTime.Now
                    });
                }

                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess, model.ItemId.ToString());
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateItemRecipe(ItemRecipeUpsertDto model)
        {
            var validationError = await ValidateRecipeModelAsync(model, true);
            if (validationError != null)
                return ApiResponseModel<string>.Failure(validationError);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingRecipes = await _unitOfWork.Repository<ItemRecipe>()
                    .GetAllAsQueryable()
                    .Where(i => i.ItemId == model.ItemId)
                    .ToListAsync();

                if (existingRecipes.Count == 0)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                _unitOfWork.Repository<ItemRecipe>().DeleteRange(existingRecipes);

                foreach (var recipeLine in model.Recipes)
                {
                    await _unitOfWork.Repository<ItemRecipe>().AddAsync(new ItemRecipe
                    {
                        ItemId = model.ItemId,
                        InventoryItemId = recipeLine.InventoryItemId,
                        QuantityNeeded = recipeLine.QuantityNeeded,
                        InsertUser = model.UserId,
                        InsertDate = DateTime.Now,
                        UpdateUser = model.UserId,
                        UpdateDate = DateTime.Now
                    });
                }

                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess, model.ItemId.ToString());
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> DeleteItemRecipe(int itemId)
        {
            try
            {
                var recipes = await _unitOfWork.Repository<ItemRecipe>()
                    .GetAllAsQueryable()
                    .Where(i => i.ItemId == itemId)
                    .ToListAsync();

                if (recipes.Count == 0)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                _unitOfWork.Repository<ItemRecipe>().DeleteRange(recipes);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        private IQueryable<ItemRecipeLineQueryDto> BuildItemRecipeLinesQuery()
        {
            var itemRecipes = _unitOfWork.Repository<ItemRecipe>().GetAllAsQueryable().AsNoTracking();
            var items = _unitOfWork.Repository<Item>().GetAllAsQueryable().AsNoTracking();
            var inventoryItems = _unitOfWork.Repository<InventoryItem>().GetAllAsQueryable().AsNoTracking();

            return from itemRecipe in itemRecipes
                   join item in items
                       on itemRecipe.ItemId equals item.ItemId into itemGroup
                   from item in itemGroup.DefaultIfEmpty()
                   join inventoryItem in inventoryItems
                       on itemRecipe.InventoryItemId equals inventoryItem.InventoryItemId into inventoryGroup
                   from inventoryItem in inventoryGroup.DefaultIfEmpty()
                   select new ItemRecipeLineQueryDto
                   {
                       ItemRecipeId = itemRecipe.ItemRecipeId,
                       ItemId = itemRecipe.ItemId,
                       ItemName = item != null ? item.Name : string.Empty,
                       InventoryItemId = itemRecipe.InventoryItemId,
                       InventoryItemName = inventoryItem != null ? inventoryItem.Name : string.Empty,
                       InventoryItemUnitName = inventoryItem != null && inventoryItem.Unit != null ? inventoryItem.Unit.Name : string.Empty,
                       QuantityNeeded = itemRecipe.QuantityNeeded,
                       InsertUser = itemRecipe.InsertUser,
                       InsertDate = itemRecipe.InsertDate,
                       UpdateUser = itemRecipe.UpdateUser,
                       UpdateDate = itemRecipe.UpdateDate
                   };
        }

        private ItemRecipeDetailsDto BuildRecipeGroup(List<ItemRecipeLineQueryDto> lines)
        {
            return new ItemRecipeDetailsDto
            {
                ItemId = lines[0].ItemId,
                ItemName = lines[0].ItemName,
                RecipesCount = lines.Count,
                IngredientsSummary = string.Join("، ", lines.Select(i => i.InventoryItemName).Distinct()),
                InsertUser = lines.Select(i => i.InsertUser).FirstOrDefault(i => !string.IsNullOrWhiteSpace(i)),
                InsertDate = lines.Min(i => i.InsertDate),
                UpdateUser = lines.OrderByDescending(i => i.UpdateDate ?? i.InsertDate).Select(i => i.UpdateUser).FirstOrDefault(i => !string.IsNullOrWhiteSpace(i)),
                UpdateDate = lines.Max(i => i.UpdateDate ?? i.InsertDate),
                Recipes = lines.Select(i => new ItemRecipeLineDetailsDto
                {
                    ItemRecipeId = i.ItemRecipeId,
                    InventoryItemId = i.InventoryItemId,
                    InventoryItemName = i.InventoryItemName,
                    InventoryItemUnitName = i.InventoryItemUnitName,
                    QuantityNeeded = i.QuantityNeeded
                }).ToList()
            };
        }

        private async Task<Error> ValidateRecipeModelAsync(ItemRecipeUpsertDto model, bool requireExistingRecipes)
        {
            if (model == null || model.ItemId <= 0)
                return InvalidRecipeData;

            if (model.Recipes == null || model.Recipes.Count == 0)
                return RecipeLinesRequired;

            if (model.Recipes.Any(i => i.InventoryItemId <= 0))
                return InvalidRecipeData;

            if (model.Recipes.Any(i => i.QuantityNeeded <= 0))
                return InvalidRecipeQuantity;

            if (model.Recipes.GroupBy(i => i.InventoryItemId).Any(i => i.Count() > 1))
                return DuplicateRecipe;

            var itemExists = await _unitOfWork.Repository<Item>().AnyAsync(i => i.ItemId == model.ItemId);
            if (!itemExists)
                return GenericErrors.NotFound;

            var inventoryItemIds = model.Recipes.Select(i => i.InventoryItemId).Distinct().ToList();
            var inventoryItemsCount = await _unitOfWork.Repository<InventoryItem>()
                .GetAllAsQueryable()
                .CountAsync(i => inventoryItemIds.Contains(i.InventoryItemId));

            if (inventoryItemsCount != inventoryItemIds.Count)
                return GenericErrors.NotFound;

            var hasExistingRecipes = await _unitOfWork.Repository<ItemRecipe>().AnyAsync(i => i.ItemId == model.ItemId);
            if (requireExistingRecipes && !hasExistingRecipes)
                return GenericErrors.NotFound;

            if (!requireExistingRecipes && hasExistingRecipes)
                return GenericErrors.AlreadyExists;

            return null;
        }

        private static IEnumerable<T> ApplyPaging<T>(IEnumerable<T> query, PagingFilterModel model)
        {
            var currentPage = model?.Currentpage > 0 ? model.Currentpage : 1;
            var pageSize = model?.Pagesize > 0 ? model.Pagesize : 20;
            return query.Skip((currentPage - 1) * pageSize).Take(pageSize);
        }

        private static string GetFilterValue(PagingFilterModel model, string categoryName)
        {
            var filter = model?.FilterList?.FirstOrDefault(i => i.CategoryName == categoryName);
            return filter?.ItemId ?? filter?.ItemValue ?? string.Empty;
        }

        private static int? GetFilterIntValue(PagingFilterModel model, string categoryName)
        {
            var filterValue = GetFilterValue(model, categoryName);
            return int.TryParse(filterValue, out var parsedValue) ? parsedValue : null;
        }

        private class ItemRecipeLineQueryDto
        {
            public int ItemRecipeId { get; set; }
            public int ItemId { get; set; }
            public string ItemName { get; set; }
            public int InventoryItemId { get; set; }
            public string InventoryItemName { get; set; }
            public string InventoryItemUnitName { get; set; }
            public double QuantityNeeded { get; set; }
            public string InsertUser { get; set; }
            public DateTime? InsertDate { get; set; }
            public string UpdateUser { get; set; }
            public DateTime? UpdateDate { get; set; }
        }
    }
}
