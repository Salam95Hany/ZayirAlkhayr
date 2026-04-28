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
    public class InventoryAdjustmentService : IInventoryAdjustmentService
    {
        private static readonly Error InvalidInventoryQuantity = new Error("لا يمكن أن تكون الكمية المتاحة أقل من صفر");
        private readonly IUnitOfWork _unitOfWork;

        public InventoryAdjustmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<List<InventoryAdjustmentDetailsDto>>> GetAllInventoryAdjustments(PagingFilterModel model)
        {
            var searchText = GetFilterValue(model, "SearchText");
            var inventoryItemId = GetFilterIntValue(model, "InventoryItemId");
            var fromDate = GetFromDate(model);
            var toDate = GetToDate(model);

            var adjustments = _unitOfWork.Repository<InventoryAdjustment>().GetAllAsQueryable().AsNoTracking();
            var inventoryItems = _unitOfWork.Repository<InventoryItem>().GetAllAsQueryable().AsNoTracking();

            var query = from adjustment in adjustments
                        join inventoryItem in inventoryItems
                            on adjustment.InventoryItemId equals inventoryItem.InventoryItemId into inventoryGroup
                        from inventoryItem in inventoryGroup.DefaultIfEmpty()
                        select new InventoryAdjustmentDetailsDto
                        {
                            InventoryAdjustmentId = adjustment.InventoryAdjustmentId,
                            InventoryItemId = adjustment.InventoryItemId,
                            InventoryItemName = inventoryItem != null ? inventoryItem.Name : string.Empty,
                            QuantityChange = adjustment.QuantityChange,
                            Reason = adjustment.Reason,
                            InsertUser = adjustment.InsertUser,
                            InsertDate = adjustment.InsertDate,
                            UpdateUser = adjustment.UpdateUser,
                            UpdateDate = adjustment.UpdateDate
                        };

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(i => i.InventoryItemName.Contains(searchText) || i.Reason.Contains(searchText));

            if (inventoryItemId.HasValue)
                query = query.Where(i => i.InventoryItemId == inventoryItemId.Value);

            if (fromDate.HasValue)
                query = query.Where(i => i.InsertDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(i => i.InsertDate <= toDate.Value);

            query = query.OrderByDescending(i => i.InventoryAdjustmentId);
            var totalCount = await query.CountAsync();
            var results = await ApplyPaging(query, model).ToListAsync();

            return ApiResponseModel<List<InventoryAdjustmentDetailsDto>>.Success(GenericErrors.GetSuccess, results, totalCount);
        }

        public async Task<ApiResponseModel<InventoryAdjustmentDetailsDto>> GetInventoryAdjustmentById(int inventoryAdjustmentId)
        {
            var adjustments = _unitOfWork.Repository<InventoryAdjustment>().GetAllAsQueryable().AsNoTracking();
            var inventoryItems = _unitOfWork.Repository<InventoryItem>().GetAllAsQueryable().AsNoTracking();

            var result = await (from adjustment in adjustments
                                join inventoryItem in inventoryItems
                                    on adjustment.InventoryItemId equals inventoryItem.InventoryItemId into inventoryGroup
                                from inventoryItem in inventoryGroup.DefaultIfEmpty()
                                where adjustment.InventoryAdjustmentId == inventoryAdjustmentId
                                select new InventoryAdjustmentDetailsDto
                                {
                                    InventoryAdjustmentId = adjustment.InventoryAdjustmentId,
                                    InventoryItemId = adjustment.InventoryItemId,
                                    InventoryItemName = inventoryItem != null ? inventoryItem.Name : string.Empty,
                                    QuantityChange = adjustment.QuantityChange,
                                    Reason = adjustment.Reason,
                                    InsertUser = adjustment.InsertUser,
                                    InsertDate = adjustment.InsertDate,
                                    UpdateUser = adjustment.UpdateUser,
                                    UpdateDate = adjustment.UpdateDate
                                }).FirstOrDefaultAsync();

            if (result == null)
                return ApiResponseModel<InventoryAdjustmentDetailsDto>.Failure(GenericErrors.NotFound);

            return ApiResponseModel<InventoryAdjustmentDetailsDto>.Success(GenericErrors.GetSuccess, result);
        }

        public async Task<ApiResponseModel<string>> AddInventoryAdjustment(InventoryAdjustment model)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var inventoryItem = await _unitOfWork.Repository<InventoryItem>().GetByIdAsync(model.InventoryItemId);
                if (inventoryItem == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var updatedQuantity = inventoryItem.CurrentQuantity + model.QuantityChange;
                if (updatedQuantity < 0)
                    return ApiResponseModel<string>.Failure(InvalidInventoryQuantity);

                inventoryItem.CurrentQuantity = updatedQuantity;
                inventoryItem.UpdateUser = GetActionUser(model.UpdateUser, model.InsertUser);
                inventoryItem.UpdateDate = DateTime.Now;

                model.InsertDate = DateTime.Now;
                await _unitOfWork.Repository<InventoryAdjustment>().AddAsync(model);
                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();

                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess, model.InventoryAdjustmentId.ToString());
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateInventoryAdjustment(InventoryAdjustment model)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = await _unitOfWork.Repository<InventoryAdjustment>().GetByIdAsync(model.InventoryAdjustmentId);
                if (entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var inventoryItemIds = new[] { entity.InventoryItemId, model.InventoryItemId }.Distinct().ToList();
                var inventoryItems = await _unitOfWork.Repository<InventoryItem>()
                                                      .GetAllAsQueryable()
                                                      .Where(i => inventoryItemIds.Contains(i.InventoryItemId))
                                                      .ToListAsync();

                if (inventoryItems.Count != inventoryItemIds.Count)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                foreach (var inventoryItem in inventoryItems)
                {
                    var delta = 0;
                    if (inventoryItem.InventoryItemId == entity.InventoryItemId)
                        delta -= entity.QuantityChange;
                    if (inventoryItem.InventoryItemId == model.InventoryItemId)
                        delta += model.QuantityChange;

                    if (inventoryItem.CurrentQuantity + delta < 0)
                        return ApiResponseModel<string>.Failure(InvalidInventoryQuantity);
                }

                foreach (var inventoryItem in inventoryItems)
                {
                    var delta = 0;
                    if (inventoryItem.InventoryItemId == entity.InventoryItemId)
                        delta -= entity.QuantityChange;
                    if (inventoryItem.InventoryItemId == model.InventoryItemId)
                        delta += model.QuantityChange;

                    inventoryItem.CurrentQuantity += delta;
                    inventoryItem.UpdateUser = GetActionUser(model.UpdateUser, model.InsertUser);
                    inventoryItem.UpdateDate = DateTime.Now;
                }

                entity.InventoryItemId = model.InventoryItemId;
                entity.QuantityChange = model.QuantityChange;
                entity.Reason = model.Reason;
                entity.UpdateUser = GetActionUser(model.UpdateUser, model.InsertUser);
                entity.UpdateDate = DateTime.Now;

                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess, entity.InventoryAdjustmentId.ToString());
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> DeleteInventoryAdjustment(int inventoryAdjustmentId)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = await _unitOfWork.Repository<InventoryAdjustment>().GetByIdAsync(inventoryAdjustmentId);
                if (entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var inventoryItem = await _unitOfWork.Repository<InventoryItem>().GetByIdAsync(entity.InventoryItemId);
                if (inventoryItem == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var updatedQuantity = inventoryItem.CurrentQuantity - entity.QuantityChange;
                if (updatedQuantity < 0)
                    return ApiResponseModel<string>.Failure(InvalidInventoryQuantity);

                inventoryItem.CurrentQuantity = updatedQuantity;
                inventoryItem.UpdateDate = DateTime.Now;
                inventoryItem.UpdateUser = entity.UpdateUser ?? entity.InsertUser;

                _unitOfWork.Repository<InventoryAdjustment>().Delete(entity);
                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
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

        private static int? GetFilterIntValue(PagingFilterModel model, string categoryName)
        {
            var filterValue = GetFilterValue(model, categoryName);
            return int.TryParse(filterValue, out var parsedValue) ? parsedValue : null;
        }

        private static DateTime? GetFromDate(PagingFilterModel model)
        {
            var rawValue = model?.FilterList?.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            return DateTime.TryParse(rawValue, out var parsedDate) ? parsedDate.Date : null;
        }

        private static DateTime? GetToDate(PagingFilterModel model)
        {
            var rawValue = model?.FilterList?.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            return DateTime.TryParse(rawValue, out var parsedDate) ? parsedDate.Date.AddDays(1).AddTicks(-1) : null;
        }

        private static string GetActionUser(string updateUser, string insertUser)
        {
            return !string.IsNullOrWhiteSpace(updateUser) ? updateUser : insertUser;
        }
    }
}
