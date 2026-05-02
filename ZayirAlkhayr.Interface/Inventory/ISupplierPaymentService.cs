using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.Inventory
{
    public interface ISupplierPaymentService
    {
        Task<ApiResponseModel<DataTable>> GetAllSupplierInvicesData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<List<FilterModel>>> GetAllSupplierInvicesFilters(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<DataTable>> GetSupplierPaymentLogData(PagingFilterModel PagingFilter);
        Task<ApiResponseModel<string>> GetSupplierNameById(int SupplierId);
        Task<ApiResponseModel<List<PurchaseNumbersDto>>> GetPurchaseNumberBySupplierId(int SupplierId);
        Task<ApiResponseModel<string>> AddNewSupplierPayment(SupplierPayment Model);
        Task<ApiResponseModel<string>> UpdateSupplierPayment(SupplierPayment Model);
        Task<ApiResponseModel<string>> DeleteSupplierPayment(int SupplierPaymentId);
    }
}
