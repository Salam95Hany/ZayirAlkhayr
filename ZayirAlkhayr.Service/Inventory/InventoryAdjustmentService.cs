using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Entities.Specifications.Inventory;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.Inventory;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Inventory
{
    public class InventoryAdjustmentService : IInventoryAdjustmentService
    {
        private static readonly Error InvalidInventoryQuantity = new Error("لا يمكن أن تكون الكمية المتاحة أقل من صفر");
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISQLHelper _sQLHelper;

        public InventoryAdjustmentService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper)
        {
            _unitOfWork = unitOfWork;
            _sQLHelper = sQLHelper;
        }

        public async Task<ApiResponseModel<DataTable>> GetAllInventoryAdjustmentData(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@IsFilter", false);
            Params[3] = new SqlParameter("@FilterList", FilterDt);
            Params[4] = new SqlParameter("@FromDate", FromDate);
            Params[5] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Inv].[SP_GetAllInventoryAdjustment]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<FilterModel>>> GetAllInventoryAdjustmentFilters(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[1] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[2] = new SqlParameter("@IsFilter", true);
            Params[3] = new SqlParameter("@FilterList", FilterDt);
            Params[4] = new SqlParameter("@FromDate", FromDate);
            Params[5] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Inv].[SP_GetAllInventoryAdjustment]", Params);
            var Filters = dt.ToGroupedFilters();
            return ApiResponseModel<List<FilterModel>>.Success(GenericErrors.GetSuccess, Filters);
        }

        public async Task<ApiResponseModel<List<InventoryAdjustmentDetailsModelDto>>> GetAdjustmentDetailsById(int inventoryAdjustmentId)
        {
            try
            {
                var Spec = new InventoryAdjustmentDetailsSpecification(inventoryAdjustmentId);
                var Results = await _unitOfWork.Repository<InventoryAdjustmentDetail>().GetAllWithSpecAsync(Spec);
                var Data = Results.Select(r => new InventoryAdjustmentDetailsModelDto
                {
                    ItemName = r.InventoryItem.Name,
                    QuantityBefore = r.QuantityBefore,
                    QuantityAfter = r.QuantityAfter,
                    QuantityChange = r.QuantityChange,
                    TotalQuantityChange = Results.Sum(d => d.QuantityChange)
                }).ToList();

                return ApiResponseModel<List<InventoryAdjustmentDetailsModelDto>>.Success(GenericErrors.GetSuccess, Data);
            }
            catch (Exception)
            {
                return ApiResponseModel<List<InventoryAdjustmentDetailsModelDto>>.Failure(GenericErrors.TransFailed);
            }

        }


        public async Task<ApiResponseModel<InventoryAdjustmentDto>> GetInventoryAdjustmentById(int inventoryAdjustmentId)
        {
            try
            {
                var Spec = new InventoryAdjustmentByIdSpecification(inventoryAdjustmentId);
                var entity = await _unitOfWork.Repository<InventoryAdjustment>().GetByIdWithSpecAsync(Spec);
                var Data = new InventoryAdjustmentDto
                {
                    InventoryAdjustmentId = entity.InventoryAdjustmentId,
                    InventoryItemId = entity.Details.FirstOrDefault().InventoryItemId,
                    Reason = entity.Reason,
                    QuantityChange = entity.Details.FirstOrDefault().QuantityChange
                };

                return ApiResponseModel<InventoryAdjustmentDto>.Success(GenericErrors.GetSuccess, Data);
            }
            catch (Exception)
            {
                return ApiResponseModel<InventoryAdjustmentDto>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> AddInventoryAdjustment(InventoryAdjustmentRequest model)
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
                inventoryItem.UpdateUser = model.InsertUser;
                inventoryItem.UpdateDate = DateTime.Now;

                var adjustment = new InventoryAdjustment
                {
                    ActionId = "-",
                    AdjustmentType = AdjustmentTypes.Manual,
                    Reason = model.Reason,
                    TotalAffectedItems = 1,
                    InsertUser = model.InsertUser,
                    InsertDate = DateTime.Now
                };

                await _unitOfWork.Repository<InventoryAdjustment>().AddAsync(adjustment);
                await _unitOfWork.CompleteAsync();

                var detail = new InventoryAdjustmentDetail
                {
                    InventoryAdjustmentId = adjustment.InventoryAdjustmentId,
                    InventoryItemId = model.InventoryItemId,
                    QuantityBefore = inventoryItem.CurrentQuantity,
                    QuantityAfter = updatedQuantity,
                    QuantityChange = model.QuantityChange
                };

                await _unitOfWork.Repository<InventoryAdjustmentDetail>().AddAsync(detail);
                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();

                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateInventoryAdjustment(InventoryAdjustmentRequest model)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Spec  = new InventoryAdjustmentByIdSpecification(model.InventoryAdjustmentId);
                var entity = await _unitOfWork.Repository<InventoryAdjustment>().GetByIdWithSpecAsync(Spec);
                if (entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var entityInvItem = entity.Details.FirstOrDefault();
                var inventoryItemIds = new[] { entityInvItem.InventoryItemId, model.InventoryItemId }.Distinct().ToList();
                var inventoryItems = await _unitOfWork.Repository<InventoryItem>()
                                                      .GetAllAsQueryable()
                                                      .Where(i => inventoryItemIds.Contains(i.InventoryItemId))
                                                      .ToListAsync();

                if (inventoryItems.Count != inventoryItemIds.Count)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                foreach (var inventoryItem in inventoryItems)
                {
                    double delta = 0;
                    if (inventoryItem.InventoryItemId == entityInvItem.InventoryItemId)
                        delta -= entityInvItem.QuantityChange;
                    if (inventoryItem.InventoryItemId == model.InventoryItemId)
                        delta += model.QuantityChange;

                    if (inventoryItem.CurrentQuantity + delta < 0)
                        return ApiResponseModel<string>.Failure(InvalidInventoryQuantity);
                }

                foreach (var inventoryItem in inventoryItems)
                {
                    double delta = 0;
                    if (inventoryItem.InventoryItemId == entityInvItem.InventoryItemId)
                        delta -= entityInvItem.QuantityChange;
                    if (inventoryItem.InventoryItemId == model.InventoryItemId)
                        delta += model.QuantityChange;

                    inventoryItem.CurrentQuantity += delta;
                    inventoryItem.UpdateUser = model.InsertUser;
                    inventoryItem.UpdateDate = DateTime.Now;
                }

                entityInvItem.InventoryItemId = model.InventoryItemId;
                entityInvItem.QuantityChange = model.QuantityChange;
                entity.Reason = model.Reason;
                entity.UpdateUser = model.InsertUser;
                entity.UpdateDate = DateTime.Now;

                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
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
                var Spec = new InventoryAdjustmentByIdSpecification(inventoryAdjustmentId);
                var entity = await _unitOfWork.Repository<InventoryAdjustment>().GetByIdWithSpecAsync(Spec);
                if (entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var entityInvItem = entity.Details.FirstOrDefault()?.InventoryItem;
                var entityDetail = entity.Details.FirstOrDefault();
                if (entityInvItem == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var updatedQuantity = entityInvItem.CurrentQuantity - entityDetail.QuantityChange;
                if (updatedQuantity < 0)
                    return ApiResponseModel<string>.Failure(InvalidInventoryQuantity);

                entityInvItem.CurrentQuantity = updatedQuantity;
                entityInvItem.UpdateDate = DateTime.Now;
                entityInvItem.UpdateUser = entity.UpdateUser ?? entity.InsertUser;

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
    }
}
