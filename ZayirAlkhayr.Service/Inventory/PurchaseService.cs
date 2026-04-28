using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Inventory;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.Inventory
{
    public class PurchaseService : IPurchaseService
    {
        private static readonly Error InvalidInventoryQuantity = new Error("لا يمكن أن تكون الكمية المتاحة أقل من صفر");
        private static readonly Error InvalidPurchaseData = new Error("بيانات الشراء غير صحيحة");
        private static readonly Error PurchaseItemsRequired = new Error("يجب إضافة أصناف للشراء");
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponseModel<List<PurchaseSummaryDto>>> GetAllPurchases(PagingFilterModel model)
        {
            var searchText = GetFilterValue(model, "SearchText");
            var supplierId = GetFilterIntValue(model, "SupplierId");
            var fromDate = GetFromDate(model);
            var toDate = GetToDate(model);

            var purchases = _unitOfWork.Repository<Purchase>().GetAllAsQueryable().AsNoTracking();
            var suppliers = _unitOfWork.Repository<Supplier>().GetAllAsQueryable().AsNoTracking();
            var purchaseItems = _unitOfWork.Repository<PurchaseItem>().GetAllAsQueryable().AsNoTracking();

            var query = from purchase in purchases
                        join supplier in suppliers
                            on purchase.SupplierId equals supplier.SupplierId into supplierGroup
                        from supplier in supplierGroup.DefaultIfEmpty()
                        select new PurchaseSummaryDto
                        {
                            PurchaseId = purchase.PurchaseId,
                            SupplierId = purchase.SupplierId,
                            SupplierName = supplier != null ? supplier.Name : string.Empty,
                            TotalAmount = purchase.TotalAmount,
                            ItemsCount = purchaseItems.Count(i => i.PurchaseId == purchase.PurchaseId),
                            InsertUser = purchase.InsertUser,
                            InsertDate = purchase.InsertDate,
                            UpdateUser = purchase.UpdateUser,
                            UpdateDate = purchase.UpdateDate
                        };

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(i => i.SupplierName.Contains(searchText) || i.InsertUser.Contains(searchText));

            if (supplierId.HasValue)
                query = query.Where(i => i.SupplierId == supplierId.Value);

            if (fromDate.HasValue)
                query = query.Where(i => i.InsertDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(i => i.InsertDate <= toDate.Value);

            query = query.OrderByDescending(i => i.PurchaseId);
            var totalCount = await query.CountAsync();
            var results = await ApplyPaging(query, model).ToListAsync();

            return ApiResponseModel<List<PurchaseSummaryDto>>.Success(GenericErrors.GetSuccess, results, totalCount);
        }

        public async Task<ApiResponseModel<PurchaseDetailsDto>> GetPurchaseById(int purchaseId)
        {
            var purchase = await _unitOfWork.Repository<Purchase>()
                                            .GetAllAsQueryable()
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(i => i.PurchaseId == purchaseId);
            if (purchase == null)
                return ApiResponseModel<PurchaseDetailsDto>.Failure(GenericErrors.NotFound);

            var supplier = await _unitOfWork.Repository<Supplier>()
                                            .GetAllAsQueryable()
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(i => i.SupplierId == purchase.SupplierId);

            var items = await GetPurchaseItemsDetailsAsync(purchaseId);
            var data = new PurchaseDetailsDto
            {
                PurchaseId = purchase.PurchaseId,
                SupplierId = purchase.SupplierId,
                SupplierName = supplier?.Name ?? string.Empty,
                SupplierPhone = supplier?.Phone ?? string.Empty,
                TotalAmount = purchase.TotalAmount,
                InsertUser = purchase.InsertUser,
                InsertDate = purchase.InsertDate,
                UpdateUser = purchase.UpdateUser,
                UpdateDate = purchase.UpdateDate,
                Items = items
            };

            return ApiResponseModel<PurchaseDetailsDto>.Success(GenericErrors.GetSuccess, data);
        }

        public async Task<ApiResponseModel<string>> AddPurchase(PurchaseUpsertDto model)
        {
            if (!IsValidPurchaseModel(model))
                return ApiResponseModel<string>.Failure(model?.Items?.Count > 0 ? InvalidPurchaseData : PurchaseItemsRequired);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(model.SupplierId);
                if (supplier == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var requestedIds = model.Items.Select(i => i.InventoryItemId).Distinct().ToList();
                var inventoryItems = await LoadInventoryItemsAsync(requestedIds);
                if (inventoryItems.Count != requestedIds.Count)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                foreach (var item in model.Items)
                {
                    inventoryItems[item.InventoryItemId].CurrentQuantity += item.Quantity;
                    inventoryItems[item.InventoryItemId].UpdateUser = model.UserId;
                    inventoryItems[item.InventoryItemId].UpdateDate = DateTime.Now;
                }

                var purchase = new Purchase
                {
                    SupplierId = model.SupplierId,
                    TotalAmount = model.Items.Sum(i => i.Quantity * i.CostPrice),
                    InsertUser = model.UserId,
                    InsertDate = DateTime.Now
                };

                await _unitOfWork.Repository<Purchase>().AddAsync(purchase);
                await _unitOfWork.CompleteAsync();

                foreach (var item in model.Items)
                {
                    await _unitOfWork.Repository<PurchaseItem>().AddAsync(new PurchaseItem
                    {
                        PurchaseId = purchase.PurchaseId,
                        InventoryItemId = item.InventoryItemId,
                        Quantity = item.Quantity,
                        CostPrice = item.CostPrice,
                        Total = item.Quantity * item.CostPrice,
                        InsertUser = model.UserId,
                        InsertDate = DateTime.Now
                    });
                }

                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess, purchase.PurchaseId.ToString());
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdatePurchase(PurchaseUpsertDto model)
        {
            if (!model.PurchaseId.HasValue)
                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

            if (!IsValidPurchaseModel(model))
                return ApiResponseModel<string>.Failure(model?.Items?.Count > 0 ? InvalidPurchaseData : PurchaseItemsRequired);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var purchase = await _unitOfWork.Repository<Purchase>().GetByIdAsync(model.PurchaseId.Value);
                if (purchase == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(model.SupplierId);
                if (supplier == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var existingPurchaseItems = await _unitOfWork.Repository<PurchaseItem>()
                                                            .GetAllAsQueryable()
                                                            .Where(i => i.PurchaseId == purchase.PurchaseId)
                                                            .ToListAsync();

                var existingQuantityMap = existingPurchaseItems
                    .GroupBy(i => i.InventoryItemId)
                    .ToDictionary(i => i.Key, i => i.Sum(v => v.Quantity));

                var requestedQuantityMap = model.Items
                    .GroupBy(i => i.InventoryItemId)
                    .ToDictionary(i => i.Key, i => i.Sum(v => v.Quantity));

                var inventoryItemIds = existingQuantityMap.Keys.Union(requestedQuantityMap.Keys).ToList();
                var inventoryItems = await LoadInventoryItemsAsync(inventoryItemIds);
                if (inventoryItems.Count != inventoryItemIds.Count)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                foreach (var inventoryItemId in inventoryItemIds)
                {
                    var oldQuantity = existingQuantityMap.ContainsKey(inventoryItemId) ? existingQuantityMap[inventoryItemId] : 0;
                    var newQuantity = requestedQuantityMap.ContainsKey(inventoryItemId) ? requestedQuantityMap[inventoryItemId] : 0;
                    var delta = newQuantity - oldQuantity;

                    if (inventoryItems[inventoryItemId].CurrentQuantity + delta < 0)
                        return ApiResponseModel<string>.Failure(InvalidInventoryQuantity);
                }

                foreach (var inventoryItemId in inventoryItemIds)
                {
                    var oldQuantity = existingQuantityMap.ContainsKey(inventoryItemId) ? existingQuantityMap[inventoryItemId] : 0;
                    var newQuantity = requestedQuantityMap.ContainsKey(inventoryItemId) ? requestedQuantityMap[inventoryItemId] : 0;
                    var delta = newQuantity - oldQuantity;

                    inventoryItems[inventoryItemId].CurrentQuantity += delta;
                    inventoryItems[inventoryItemId].UpdateUser = model.UserId;
                    inventoryItems[inventoryItemId].UpdateDate = DateTime.Now;
                }

                foreach (var purchaseItem in existingPurchaseItems)
                    _unitOfWork.Repository<PurchaseItem>().Delete(purchaseItem);

                foreach (var item in model.Items)
                {
                    await _unitOfWork.Repository<PurchaseItem>().AddAsync(new PurchaseItem
                    {
                        PurchaseId = purchase.PurchaseId,
                        InventoryItemId = item.InventoryItemId,
                        Quantity = item.Quantity,
                        CostPrice = item.CostPrice,
                        Total = item.Quantity * item.CostPrice,
                        InsertUser = model.UserId,
                        InsertDate = DateTime.Now
                    });
                }

                purchase.SupplierId = model.SupplierId;
                purchase.TotalAmount = model.Items.Sum(i => i.Quantity * i.CostPrice);
                purchase.UpdateUser = model.UserId;
                purchase.UpdateDate = DateTime.Now;

                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess, purchase.PurchaseId.ToString());
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> DeletePurchase(int purchaseId)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var purchase = await _unitOfWork.Repository<Purchase>().GetByIdAsync(purchaseId);
                if (purchase == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var purchaseItems = await _unitOfWork.Repository<PurchaseItem>()
                                                     .GetAllAsQueryable()
                                                     .Where(i => i.PurchaseId == purchaseId)
                                                     .ToListAsync();

                var quantityMap = purchaseItems
                    .GroupBy(i => i.InventoryItemId)
                    .ToDictionary(i => i.Key, i => i.Sum(v => v.Quantity));

                var inventoryItems = await LoadInventoryItemsAsync(quantityMap.Keys);
                if (inventoryItems.Count != quantityMap.Count)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                foreach (var item in quantityMap)
                {
                    if (inventoryItems[item.Key].CurrentQuantity - item.Value < 0)
                        return ApiResponseModel<string>.Failure(InvalidInventoryQuantity);
                }

                foreach (var item in quantityMap)
                {
                    inventoryItems[item.Key].CurrentQuantity -= item.Value;
                    inventoryItems[item.Key].UpdateUser = purchase.UpdateUser ?? purchase.InsertUser;
                    inventoryItems[item.Key].UpdateDate = DateTime.Now;
                }

                if (purchaseItems.Count > 0)
                    _unitOfWork.Repository<PurchaseItem>().DeleteRange(purchaseItems);

                _unitOfWork.Repository<Purchase>().Delete(purchase);
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

        private static bool IsValidPurchaseModel(PurchaseUpsertDto model)
        {
            return model != null
                && model.SupplierId > 0
                && model.Items != null
                && model.Items.Count > 0
                && model.Items.All(i => i.InventoryItemId > 0 && i.Quantity > 0 && i.CostPrice >= 0);
        }

        private async Task<Dictionary<int, InventoryItem>> LoadInventoryItemsAsync(IEnumerable<int> inventoryItemIds)
        {
            var ids = inventoryItemIds.Distinct().ToList();
            if (ids.Count == 0)
                return new Dictionary<int, InventoryItem>();

            return await _unitOfWork.Repository<InventoryItem>()
                                    .GetAllAsQueryable()
                                    .Where(i => ids.Contains(i.InventoryItemId))
                                    .ToDictionaryAsync(i => i.InventoryItemId);
        }

        private async Task<List<PurchaseItemDetailsDto>> GetPurchaseItemsDetailsAsync(int purchaseId)
        {
            var purchaseItems = _unitOfWork.Repository<PurchaseItem>().GetAllAsQueryable().AsNoTracking();
            var inventoryItems = _unitOfWork.Repository<InventoryItem>().GetAllAsQueryable().AsNoTracking();

            return await (from purchaseItem in purchaseItems
                          join inventoryItem in inventoryItems
                              on purchaseItem.InventoryItemId equals inventoryItem.InventoryItemId into inventoryGroup
                          from inventoryItem in inventoryGroup.DefaultIfEmpty()
                          where purchaseItem.PurchaseId == purchaseId
                          orderby purchaseItem.PurchaseItemId descending
                          select new PurchaseItemDetailsDto
                          {
                              PurchaseItemId = purchaseItem.PurchaseItemId,
                              PurchaseId = purchaseItem.PurchaseId,
                              InventoryItemId = purchaseItem.InventoryItemId,
                              InventoryItemName = inventoryItem != null ? inventoryItem.Name : string.Empty,
                              UnitName = inventoryItem != null ? inventoryItem.Unit.Name : string.Empty,
                              Quantity = purchaseItem.Quantity,
                              CostPrice = purchaseItem.CostPrice,
                              Total = purchaseItem.Total,
                              InsertUser = purchaseItem.InsertUser,
                              InsertDate = purchaseItem.InsertDate,
                              UpdateUser = purchaseItem.UpdateUser,
                              UpdateDate = purchaseItem.UpdateDate
                          }).ToListAsync();
        }
    }
}
