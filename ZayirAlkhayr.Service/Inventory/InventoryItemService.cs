using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Inventory;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Inventory
{
    public class InventoryItemService : IInventoryItemService
    {
        private static readonly Error InvalidInventoryQuantity = new Error("\u0644\u0627 \u064A\u0645\u0643\u0646 \u0623\u0646 \u062A\u0643\u0648\u0646 \u0627\u0644\u0643\u0645\u064A\u0629 \u0627\u0644\u0645\u062A\u0627\u062D\u0629 \u0623\u0642\u0644 \u0645\u0646 \u0635\u0641\u0631");
        private readonly IUnitOfWork _unitOfWork;

        public InventoryItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<List<InventoryItem>>> GetAllInventoryItems(PagingFilterModel model)
        {
            var searchText = GetFilterValue(model, "SearchText");
            var lowStockOnly = IsFilterChecked(model, "LowStockOnly") || IsFilterChecked(model, "BelowMinOnly");

            var query = _unitOfWork.Repository<InventoryItem>()
                                   .GetAllAsQueryable()
                                   .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(i => i.Name.Contains(searchText));

            if (lowStockOnly)
                query = query.Where(i => i.CurrentQuantity <= i.MinQuantity);

            query = query.OrderByDescending(i => i.InventoryItemId);
            var totalCount = await query.CountAsync();
            var results = await ApplyPaging(query, model).ToListAsync();

            return ApiResponseModel<List<InventoryItem>>.Success(GenericErrors.GetSuccess, results, totalCount);
        }

        public async Task<ApiResponseModel<InventoryItem>> GetInventoryItemById(int inventoryItemId)
        {
            var entity = await _unitOfWork.Repository<InventoryItem>()
                                          .GetAllAsQueryable()
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(i => i.InventoryItemId == inventoryItemId);

            if (entity == null)
                return ApiResponseModel<InventoryItem>.Failure(GenericErrors.NotFound);

            return ApiResponseModel<InventoryItem>.Success(GenericErrors.GetSuccess, entity);
        }

        public async Task<ApiResponseModel<List<InventoryItem>>> GetLowStockInventoryItems()
        {
            var results = await _unitOfWork.Repository<InventoryItem>()
                                           .GetAllAsQueryable()
                                           .AsNoTracking()
                                           .Where(i => i.CurrentQuantity <= i.MinQuantity)
                                           .OrderBy(i => i.Name)
                                           .ToListAsync();

            return ApiResponseModel<List<InventoryItem>>.Success(GenericErrors.GetSuccess, results);
        }

        public async Task<ApiResponseModel<string>> AddNewInventoryItem(InventoryItem model)
        {
            try
            {
                if (model.CurrentQuantity < 0 || model.MinQuantity < 0)
                    return ApiResponseModel<string>.Failure(InvalidInventoryQuantity);

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
                var entity = await _unitOfWork.Repository<InventoryItem>().GetByIdAsync(inventoryItemId);
                if (entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var hasAdjustment = await _unitOfWork.Repository<InventoryAdjustment>()
                                                     .AnyAsync(i => i.InventoryItemId == inventoryItemId);
                var hasPurchaseItem = await _unitOfWork.Repository<PurchaseItem>()
                                                       .AnyAsync(i => i.InventoryItemId == inventoryItemId);

                if (hasAdjustment || hasPurchaseItem)
                    return ApiResponseModel<string>.Failure(GenericErrors.DeleteRelationRow);

                _unitOfWork.Repository<InventoryItem>().Delete(entity);
                await _unitOfWork.CompleteAsync();
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
