using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Inventory;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Inventory
{
    public class InventoryItemService : IInventoryItemService
    {
        private static readonly Error InvalidInventoryQuantity = new Error("لا يمكن أن تكون الكمية المتاحة أقل من صفر");
        private static readonly Error InvalidUnit = new Error("الوحدة المحددة غير موجودة");
        private readonly IUnitOfWork _unitOfWork;

        public InventoryItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<List<InventoryItemDetailsDto>>> GetAllInventoryItems(PagingFilterModel model)
        {
            var searchText = GetFilterValue(model, "SearchText");
            var lowStockOnly = IsFilterChecked(model, "LowStockOnly") || IsFilterChecked(model, "BelowMinOnly");

            var query = BuildInventoryItemDetailsQuery();

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(i => i.Name.Contains(searchText) || i.UnitName.Contains(searchText));

            if (lowStockOnly)
                query = query.Where(i => i.CurrentQuantity <= i.MinQuantity);

            query = query.OrderByDescending(i => i.InventoryItemId);
            var totalCount = await query.CountAsync();
            var results = await ApplyPaging(query, model).ToListAsync();

            return ApiResponseModel<List<InventoryItemDetailsDto>>.Success(GenericErrors.GetSuccess, results, totalCount);
        }

        public async Task<ApiResponseModel<InventoryItemDetailsDto>> GetInventoryItemById(int inventoryItemId)
        {
            var entity = await BuildInventoryItemDetailsQuery()
                .FirstOrDefaultAsync(i => i.InventoryItemId == inventoryItemId);

            if (entity == null)
                return ApiResponseModel<InventoryItemDetailsDto>.Failure(GenericErrors.NotFound);

            return ApiResponseModel<InventoryItemDetailsDto>.Success(GenericErrors.GetSuccess, entity);
        }

        public async Task<ApiResponseModel<List<InventoryItemDetailsDto>>> GetLowStockInventoryItems()
        {
            var results = await BuildInventoryItemDetailsQuery()
                .Where(i => i.CurrentQuantity <= i.MinQuantity)
                .OrderBy(i => i.Name)
                .ToListAsync();

            return ApiResponseModel<List<InventoryItemDetailsDto>>.Success(GenericErrors.GetSuccess, results);
        }

        public async Task<ApiResponseModel<string>> AddNewInventoryItem(InventoryItem model)
        {
            try
            {
                if (model.CurrentQuantity < 0 || model.MinQuantity < 0)
                    return ApiResponseModel<string>.Failure(InvalidInventoryQuantity);

                if (!await UnitExistsAsync(model.UnitId))
                    return ApiResponseModel<string>.Failure(InvalidUnit);

                model.InsertDate = DateTime.Now;
                await _unitOfWork.Repository<InventoryItem>().AddAsync(model);
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess, model.InventoryItemId.ToString());
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateInventoryItem(InventoryItem model)
        {
            try
            {
                if (model.CurrentQuantity < 0 || model.MinQuantity < 0)
                    return ApiResponseModel<string>.Failure(InvalidInventoryQuantity);

                var entity = await _unitOfWork.Repository<InventoryItem>().GetByIdAsync(model.InventoryItemId);
                if (entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                if (!await UnitExistsAsync(model.UnitId))
                    return ApiResponseModel<string>.Failure(InvalidUnit);

                entity.Name = model.Name;
                entity.UnitId = model.UnitId;
                entity.CurrentQuantity = model.CurrentQuantity;
                entity.MinQuantity = model.MinQuantity;
                entity.UpdateUser = GetActionUser(model.UpdateUser, model.InsertUser);
                entity.UpdateDate = DateTime.Now;

                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess, entity.InventoryItemId.ToString());
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> DeleteInventoryItem(int inventoryItemId)
        {
            try
            {
                //var entity = await _unitOfWork.Repository<InventoryItem>().GetByIdAsync(inventoryItemId);
                //if (entity == null)
                //    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                //var hasAdjustment = await _unitOfWork.Repository<InventoryAdjustment>()
                //                                     .AnyAsync(i => i.InventoryItemId == inventoryItemId);
                //var hasPurchaseItem = await _unitOfWork.Repository<PurchaseItem>()
                //                                       .AnyAsync(i => i.InventoryItemId == inventoryItemId);
                //var hasRecipe = await _unitOfWork.Repository<ItemRecipe>()
                //                                 .AnyAsync(i => i.InventoryItemId == inventoryItemId);

                //if (hasAdjustment || hasPurchaseItem || hasRecipe)
                //    return ApiResponseModel<string>.Failure(GenericErrors.DeleteRelationRow);

                //_unitOfWork.Repository<InventoryItem>().Delete(entity);
                //await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        private static IQueryable<T> ApplyPaging<T>(IQueryable<T> query, PagingFilterModel model)
        {
            var currentPage = model?.Currentpage > 0 ? model.Currentpage : 1;
            var pageSize = model?.Pagesize > 0 ? model.Pagesize : 20;
            return query.Skip((currentPage - 1) * pageSize).Take(pageSize);
        }

        private IQueryable<InventoryItemDetailsDto> BuildInventoryItemDetailsQuery()
        {
            return _unitOfWork.Repository<InventoryItem>()
                .GetAllAsQueryable()
                .AsNoTracking()
                .Select(i => new InventoryItemDetailsDto
                {
                    InventoryItemId = i.InventoryItemId,
                    UnitId = i.UnitId,
                    UnitName = i.Unit != null ? i.Unit.Name : string.Empty,
                    Name = i.Name,
                    CurrentQuantity = i.CurrentQuantity,
                    MinQuantity = i.MinQuantity,
                    InsertUser = i.InsertUser,
                    InsertDate = i.InsertDate,
                    UpdateUser = i.UpdateUser,
                    UpdateDate = i.UpdateDate
                });
        }

        private async Task<bool> UnitExistsAsync(int unitId)
        {
            if (unitId <= 0)
                return false;

            return await _unitOfWork.Repository<Unit>().AnyAsync(i => i.UnitId == unitId);
        }

        private static string GetFilterValue(PagingFilterModel model, string categoryName)
        {
            var filter = model?.FilterList?.FirstOrDefault(i => i.CategoryName == categoryName);
            return filter?.ItemId ?? filter?.ItemValue ?? string.Empty;
        }

        private static bool IsFilterChecked(PagingFilterModel model, string categoryName)
        {
            return model?.FilterList?.Any(i => i.CategoryName == categoryName && i.IsChecked) == true;
        }

        private static string GetActionUser(string updateUser, string insertUser)
        {
            return !string.IsNullOrWhiteSpace(updateUser) ? updateUser : insertUser;
        }
    }
}
