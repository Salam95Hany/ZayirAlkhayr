using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.Inventory
{
    public interface IPurchaseService
    {
        Task<ApiResponseModel<List<PurchaseSummaryDto>>> GetAllPurchases(PagingFilterModel model);
        Task<ApiResponseModel<PurchaseDetailsDto>> GetPurchaseById(int purchaseId);
        Task<ApiResponseModel<string>> AddPurchase(PurchaseUpsertDto model);
        Task<ApiResponseModel<string>> UpdatePurchase(PurchaseUpsertDto model);
        Task<ApiResponseModel<string>> DeletePurchase(int purchaseId);
    }
}
