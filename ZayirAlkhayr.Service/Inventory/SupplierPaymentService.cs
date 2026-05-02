using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
    public class SupplierPaymentService: ISupplierPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISQLHelper _sQLHelper;

        public SupplierPaymentService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper)
        {
            _unitOfWork = unitOfWork;
            _sQLHelper = sQLHelper;
        }

        public async Task<ApiResponseModel<DataTable>> GetAllSupplierInvicesData(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[7];
            Params[0] = new SqlParameter("@SupplierId", PagingFilter.SupplierId);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", false);
            Params[4] = new SqlParameter("@FilterList", FilterDt);
            Params[5] = new SqlParameter("@FromDate", FromDate);
            Params[6] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Inv].[SP_GetAllSupplierInvicesData]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<FilterModel>>> GetAllSupplierInvicesFilters(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[7];
            Params[0] = new SqlParameter("@SupplierId", PagingFilter.SupplierId);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@IsFilter", true);
            Params[4] = new SqlParameter("@FilterList", FilterDt);
            Params[5] = new SqlParameter("@FromDate", FromDate);
            Params[6] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Inv].[SP_GetAllSupplierInvicesData]", Params);
            var Filters = dt.ToGroupedFilters();
            return ApiResponseModel<List<FilterModel>>.Success(GenericErrors.GetSuccess, Filters);
        }

        public async Task<ApiResponseModel<DataTable>> GetSupplierPaymentLogData(PagingFilterModel PagingFilter)
        {
            var FromDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.From;
            var ToDate = PagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "DateRange")?.To;
            var FilterDt = PagingFilter.FilterList.ToDataTableFromFilterModel();
            var Params = new SqlParameter[6];
            Params[0] = new SqlParameter("@SupplierId", PagingFilter.SupplierId);
            Params[1] = new SqlParameter("@CurrentPage", PagingFilter.Currentpage);
            Params[2] = new SqlParameter("@PageSize", PagingFilter.Pagesize);
            Params[3] = new SqlParameter("@FilterList", FilterDt);
            Params[4] = new SqlParameter("@FromDate", FromDate);
            Params[5] = new SqlParameter("@ToDate", ToDate);
            var dt = await _sQLHelper.ExecuteDataTableAsync("[Inv].[SP_GetSupplierPaymentLogData]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<string>> GetSupplierNameById(int SupplierId)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<Supplier>().GetByIdAsync(SupplierId);
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess,Entity.Name);
            }
            catch (Exception ez)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<List<PurchaseNumbersDto>>> GetPurchaseNumberBySupplierId(int SupplierId)
        {
            try
            {
                var Spec = new PurchaseNumbersBySupplierIdSpecification(SupplierId);
                var Spec2 = new SupplierPaymentBySupplierIdSpecification(SupplierId);
                var Entity = await _unitOfWork.Repository<Purchase>().GetAllWithSpecAsync(Spec);
                var Entity2 = await _unitOfWork.Repository<SupplierPayment>().GetAllWithSpecAsync(Spec2);
                var Data = Entity.Select(i => new PurchaseNumbersDto { 
                    PurchaseId = i.PurchaseId, 
                    PurchaseNumber = i.PurchaseNumber,
                    RemainingAmount = i.TotalAmount - Entity2.Where(j => j.PurchaseId == i.PurchaseId).Sum(j => j.AmountPaid)
                }).ToList();
                return ApiResponseModel<List<PurchaseNumbersDto>>.Success(GenericErrors.AddSuccess, Data);
            }
            catch (Exception ez)
            {
                return ApiResponseModel<List<PurchaseNumbersDto>>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> AddNewSupplierPayment(SupplierPayment Model)
        {
            try
            {
                Model.PaymentMethod = "كاش";
                Model.InsertDate = DateTime.Now;
                await _unitOfWork.Repository<SupplierPayment>().AddAsync(Model);
                await _unitOfWork.CompleteAsync();
                Model.PaymentNumber = $"PYI-{Model.SupplierPaymentId:D6}";
                await _unitOfWork.CompleteAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception ez)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateSupplierPayment(SupplierPayment Model)
        {
            try
            {
                var Entity = await _unitOfWork.Repository<SupplierPayment>().GetByIdAsync(Model.SupplierPaymentId);
                if (Entity != null)
                {
                    Entity.UpdateUser = Model.InsertUser;
                    Entity.UpdateDate = DateTime.Now;
                    await _unitOfWork.CompleteAsync();
                    return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess);
                }

                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

            }
            catch (Exception ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> DeleteSupplierPayment(int SupplierPaymentId)
        {
            try
            {
                var entity = await _unitOfWork.Repository<SupplierPayment>().GetByIdAsync(SupplierPaymentId);
                if (entity != null)
                {
                    _unitOfWork.Repository<SupplierPayment>().Delete(entity);
                    await _unitOfWork.CompleteAsync();
                    return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
                }

                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

            }
            catch (Exception ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
