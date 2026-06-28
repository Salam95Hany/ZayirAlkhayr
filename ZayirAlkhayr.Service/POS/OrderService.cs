using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Orders;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Entities.Specifications.Orders;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.POS;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Service.POS
{
    public class OrderService : IOrderService
    {
        private static readonly Error InvalidOrderDetails = new Error("برجاء إضافة عناصر صالحة للطلب");
        private static readonly Error InsufficientInventoryForOrder = new Error("مخزون المكونات غير كافٍ لإتمام الطلب");
        private static readonly Error OrderAlreadyCancelled = new Error("تم إلغاء هذا الطلب بالفعل");
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppSettings _appSettings;
        private readonly ISQLHelper _sQLHelper;
        private readonly string ApiLocalUrl;

        public OrderService(IUnitOfWork unitOfWork, IAppSettings appSettings, ISQLHelper sQLHelper)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings;
            ApiLocalUrl = _appSettings.ApiUrlLocal;
            _sQLHelper = sQLHelper;
        }

        public async Task<ApiResponseModel<DataTable>> GetAllOrders(PagingFilterModel PagingFilter)
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
            var dt = await _sQLHelper.ExecuteDataTableAsync("[POS].[SP_GetAllOrderDataWithFilter]", Params);
            return ApiResponseModel<DataTable>.Success(GenericErrors.GetSuccess, dt);
        }

        public async Task<ApiResponseModel<List<FilterModel>>> GetAllOrderFilters(PagingFilterModel PagingFilter)
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
            var dt = await _sQLHelper.ExecuteDataTableAsync("[POS].[SP_GetAllOrderDataWithFilter]", Params);
            var Filters = dt.ToGroupedFilters();
            return ApiResponseModel<List<FilterModel>>.Success(GenericErrors.GetSuccess, Filters);
        }

        public async Task<ApiResponseModel<OrderDetailsWithCustomer>> GetOrderDetailsByOrderId(int OrderId)
        {
            var Spec = new OrderDetailsSpecification(OrderId);
            var results = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(Spec);
            if (results == null)
                return ApiResponseModel<OrderDetailsWithCustomer>.Failure(GenericErrors.NotFound);

            var data = new OrderDetailsWithCustomer
            {
                Note = results.Note,
                VoidReason = results.VoidReason,
                Customer = new CustomerOrderResponse
                {
                    CustomerId = results.Customers?.CustomerId,
                    FullName = results.Customers?.FullName,
                    Phone = results.Customers?.Phone,
                    Address = results.Customers?.Address
                },
                OrderDetails = results.OrderDetails.Select(i => new OrderDetailsResponse
                {
                    ProductId = i.Item.ItemId,
                    ProductName = i.Item.Name,
                    CategoryName = i.Item.Category.Name,
                    Image = Path.Combine(ApiLocalUrl, "Images", ImageFiles.Items.ToString(), i.Item.Image ?? string.Empty),
                    Price = i.Item.Price,
                    UpdatedPrice = i.Price,
                    Quantity = i.Quantity,
                    TotalValue = i.Price * i.Quantity,
                }).ToList()
            };

            return ApiResponseModel<OrderDetailsWithCustomer>.Success(GenericErrors.GetSuccess, data);
        }

        public async Task<ApiResponseModel<string>> AddNewOrder(OrderWithDetailsDto Model)
        {
            if (!IsValidOrderModel(Model))
                return ApiResponseModel<string>.Failure(InvalidOrderDetails);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var OrderNumber = await _unitOfWork.Repository<Order>().MaxAsync(i =>
                    i.InsertDate.Value.Year == DateTime.Now.Year
                    && i.InsertDate.Value.Month == DateTime.Now.Month
                    && i.InsertDate.Value.Day == DateTime.Now.Day,
                    i => (int?)i.OrderNumber) ?? 0;

                var recipeConsumption = await BuildRecipeConsumptionAsync(Model.Details.Select(i => (i.ProductID, i.Quantity)));
                var inventoryUpdateError = await ApplyInventoryDeltaAsync(ToInventoryDelta(recipeConsumption, -1), Model.UserId);
                if (inventoryUpdateError != null)
                    return ApiResponseModel<string>.Failure(inventoryUpdateError);

                var order = new Order
                {
                    CustomerId = Model.CustomerId,
                    TableId = Model.TableId,
                    OrderNumber = OrderNumber + 1,
                    OrderType = Model.OrderType,
                    OrderStatus = OrderStatus.Completed,
                    TotalAmount = Model.TotalAmount,
                    CostDelivery = Model.CostDelivery,
                    IsUpdated = false,
                    Note = Model.Note,
                    InsertUser = Model.UserId,
                    InsertDate = DateTime.Now
                };

                await _unitOfWork.Repository<Order>().AddAsync(order);
                await _unitOfWork.CompleteAsync();
                var InventoryDelta = ToInventoryDelta(recipeConsumption, -1);
                if (InventoryDelta != null && InventoryDelta.Count > 0)
                    await AddInventoryAdjustment(InventoryDelta, Model.UserId, order.OrderNumber.ToString(), "خصم مخزون بسبب إنشاء طلب");

                foreach (var detail in Model.Details)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ItemId = detail.ProductID,
                        Quantity = detail.Quantity,
                        Price = detail.UnitPrice,
                        Total = detail.UnitPrice * detail.Quantity
                    };
                    await _unitOfWork.Repository<OrderDetail>().AddAsync(orderDetail);
                }

                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess, order.OrderNumber.ToString());
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateOrder(OrderWithDetailsDto order)
        {
            if (order?.OrderId == null)
                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

            if (!IsValidOrderModel(order))
                return ApiResponseModel<string>.Failure(InvalidOrderDetails);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Spec = new OrderDetailsSpecification(order.OrderId.Value);
                var entity = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(Spec);
                if (entity == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                var existingConsumption = await BuildRecipeConsumptionAsync(entity.OrderDetails.Select(i => (i.ItemId, i.Quantity)));
                var requestedConsumption = await BuildRecipeConsumptionAsync(order.Details.Select(i => (i.ProductID, i.Quantity)));
                var inventoryUpdateError = await ApplyInventoryDeltaAsync(BuildInventoryDelta(existingConsumption, requestedConsumption), order.UserId);
                if (inventoryUpdateError != null)
                    return ApiResponseModel<string>.Failure(inventoryUpdateError);

                foreach (var item in entity.OrderDetails)
                    _unitOfWork.Repository<OrderDetail>().Delete(item);

                foreach (var detail in order.Details)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId.Value,
                        ItemId = detail.ProductID,
                        Quantity = detail.Quantity,
                        Price = detail.UnitPrice,
                        Total = detail.UnitPrice * detail.Quantity
                    };
                    await _unitOfWork.Repository<OrderDetail>().AddAsync(orderDetail);
                }

                entity.TotalAmount = order.TotalAmount;
                entity.Note = order.Note;
                entity.TableId = order.TableId;
                entity.OrderType = order.OrderType;
                entity.CostDelivery = order.CostDelivery;
                entity.CustomerId = order.CustomerId;
                entity.IsUpdated = true;
                entity.UpdateUser = order.UserId;
                entity.UpdateDate = DateTime.Now;
                var InventoryDelta = BuildInventoryDelta(existingConsumption, requestedConsumption);
                if (InventoryDelta != null && InventoryDelta.Count > 0)
                    await AddInventoryAdjustment(InventoryDelta, order.UserId, entity.OrderNumber.ToString(), "خصم مخزون بسبب تعديل طلب");
                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                return ApiResponseModel<string>.Success(GenericErrors.UpdateSuccess, entity.OrderNumber.ToString());
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> CancelOrder(string VoidReason, string Action, string VoidNotes, int OrderId)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var orderSpecification = new OrderDetailsSpecification(OrderId);
                var Order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(orderSpecification);

                if (Order == null)
                    return ApiResponseModel<string>.Failure(GenericErrors.NotFound);

                if (Order.OrderStatus == OrderStatus.Cancelled)
                    return ApiResponseModel<string>.Failure(OrderAlreadyCancelled);

                var recipeConsumption = await BuildRecipeConsumptionAsync(Order.OrderDetails.Select(i => (i.ItemId, i.Quantity)));
                var inventoryUpdateError = await ApplyInventoryDeltaAsync(ToInventoryDelta(recipeConsumption, 1), Order.UpdateUser ?? Order.InsertUser);
                if (inventoryUpdateError != null)
                    return ApiResponseModel<string>.Failure(inventoryUpdateError);

                var inventoryDelta = ToInventoryDelta(recipeConsumption, 1);

                Order.VoidReason = VoidReason;
                Order.VoidNotes = VoidNotes;
                Order.OrderStatus = OrderStatus.Cancelled;
                Order.UpdateUser = Order.UpdateUser ?? Order.InsertUser;
                Order.UpdateDate = DateTime.Now;

                if (inventoryDelta != null && inventoryDelta.Count > 0)
                    await AddInventoryAdjustment(inventoryDelta, Order.UpdateUser ?? Order.InsertUser, Order.OrderNumber.ToString(), "إلغاء طلب - إعادة المخزون");
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

        public async Task<ApiResponseModel<OrderWithDetailsResponse>> GetOrderWithDetailsByOrderId(int OrderId)
        {
            var Spec = new OrderDetailsSpecification(OrderId);
            var results = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(Spec);
            if (results == null)
                return ApiResponseModel<OrderWithDetailsResponse>.Failure(GenericErrors.NotFound);

            var data = new OrderWithDetailsResponse
            {
                Notes = results.Note,
                TotalValue = results.TotalAmount,
                OrderType = results.OrderType,
                Customer = results.Customers == null ? null : new CustomerOrderResponse
                {
                    CustomerId = results.Customers.CustomerId,
                    FullName = results.Customers.FullName,
                    Phone = results.Customers.Phone,
                    Address = results.Customers.Address
                },
                OrderDetails = results.OrderDetails.Select(i => new OrderDetailsResponse
                {
                    ProductId = i.Item.ItemId,
                    ProductName = i.Item.Name,
                    Image = Path.Combine(ApiLocalUrl, "Images", ImageFiles.Items.ToString(), i.Item.Image ?? string.Empty),
                    Price = i.Item.Price,
                    Quantity = i.Quantity,
                    TotalValue = i.Item.Price * i.Quantity,
                }).ToList()
            };

            return ApiResponseModel<OrderWithDetailsResponse>.Success(GenericErrors.GetSuccess, data);
        }

        public async Task<ApiResponseModel<List<OrderWithDetailsResponse>>> GetCustomerOrdersHistory(int CustomerId)
        {
            var Spec = new CustomerOrdersSpecification(CustomerId);
            var results = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            var data = new List<OrderWithDetailsResponse>();
            if (results != null && results.Count > 0)
            {
                foreach (var item in results)
                {
                    var Obj = new OrderWithDetailsResponse();
                    Obj.OrderNumber = item.OrderNumber;
                    Obj.Notes = item.Note;
                    Obj.TotalValue = item.TotalAmount;
                    Obj.OrderType = item.OrderType;
                    Obj.OrderStatus = item.OrderStatus;
                    Obj.OrderDate = item.InsertDate.Value;
                    Obj.CashierName = item.User?.UserName;
                    Obj.OrderDetails = item.OrderDetails.Select(i => new OrderDetailsResponse
                    {
                        ProductName = i.Item.Name,
                        CategoryName = i.Item.Category.Name,
                        Image = Path.Combine(ApiLocalUrl, "Images", ImageFiles.Items.ToString(), i.Item.Image ?? string.Empty),
                        Price = i.Item.Price,
                        Quantity = i.Quantity,
                        TotalValue = i.Item.Price * i.Quantity,
                    }).ToList();

                    data.Add(Obj);
                }
            }

            return ApiResponseModel<List<OrderWithDetailsResponse>>.Success(GenericErrors.GetSuccess, data);
        }

        private static bool IsValidOrderModel(OrderWithDetailsDto order)
        {
            return order != null
                && order.Details != null
                && order.Details.Count > 0
                && order.Details.All(i => i.ProductID > 0 && i.Quantity > 0);
        }

        private async Task<Dictionary<int, double>> BuildRecipeConsumptionAsync(IEnumerable<(int ItemId, int Quantity)> orderLines)
        {
            var normalizedLines = orderLines
                .Where(i => i.ItemId > 0 && i.Quantity > 0)
                .GroupBy(i => i.ItemId)
                .ToDictionary(i => i.Key, i => i.Sum(v => v.Quantity));

            if (normalizedLines.Count == 0)
                return new Dictionary<int, double>();

            var itemIds = normalizedLines.Keys.ToList();

            var recipes = await _unitOfWork.Repository<ItemRecipe>()
                .GetAllAsQueryable()
                .AsNoTracking()
                .Where(i => itemIds.Contains(i.ItemId))
                .ToListAsync();

            if (recipes.Count == 0)
                return new Dictionary<int, double>();

            return recipes
                .GroupBy(i => i.InventoryItemId)
                .ToDictionary(
                    i => i.Key,
                    i => i.Sum(v => v.QuantityNeeded * normalizedLines[v.ItemId]));
        }

        private static Dictionary<int, double> ToInventoryDelta(Dictionary<int, double> consumptionMap, int multiplier)
        {
            return consumptionMap.ToDictionary(i => i.Key, i => i.Value * multiplier);
        }

        private static Dictionary<int, double> BuildInventoryDelta(Dictionary<int, double> existingConsumption, Dictionary<int, double> requestedConsumption)
        {
            var inventoryIds = existingConsumption.Keys.Union(requestedConsumption.Keys).Distinct().ToList();
            var delta = new Dictionary<int, double>();

            foreach (var inventoryId in inventoryIds)
            {
                var existingValue = existingConsumption.ContainsKey(inventoryId) ? existingConsumption[inventoryId] : 0;
                var requestedValue = requestedConsumption.ContainsKey(inventoryId) ? requestedConsumption[inventoryId] : 0;
                var change = existingValue - requestedValue;

                if (Math.Abs(change) > 0.0001)
                    delta[inventoryId] = change;
            }

            return delta;
        }

        private async Task<Error> ApplyInventoryDeltaAsync(Dictionary<int, double> inventoryDelta, string userId)
        {
            if (inventoryDelta == null || inventoryDelta.Count == 0)
                return null;

            var inventoryIds = inventoryDelta.Keys.ToList();

            var inventoryItems = await _unitOfWork.Repository<InventoryItem>()
                .GetAllAsQueryable()
                .Where(i => inventoryIds.Contains(i.InventoryItemId))
                .ToDictionaryAsync(i => i.InventoryItemId);

            if (inventoryItems.Count != inventoryDelta.Count)
                return GenericErrors.NotFound;

            foreach (var change in inventoryDelta)
            {
                inventoryItems[change.Key].CurrentQuantity += change.Value;
                inventoryItems[change.Key].UpdateUser = userId;
                inventoryItems[change.Key].UpdateDate = DateTime.Now;
            }

            return null;
        }


        private async Task AddInventoryAdjustment(Dictionary<int, double> inventoryDelta, string userId, string orderNumber, string Reason)
        {
            if (inventoryDelta == null || !inventoryDelta.Any())
                return;

            var adjustment = new InventoryAdjustment
            {
                ActionId = orderNumber,
                AdjustmentType = AdjustmentTypes.Order,
                Reason = Reason,
                TotalAffectedItems = inventoryDelta.Count,
                InsertUser = userId,
                InsertDate = DateTime.Now,
                Details = new List<InventoryAdjustmentDetail>()
            };

            var inventoryIds = inventoryDelta.Keys.ToList();
            var inventoryItems = await _unitOfWork.Repository<InventoryItem>().GetAllAsQueryable().Where(i => inventoryIds.Contains(i.InventoryItemId)).ToDictionaryAsync(i => i.InventoryItemId);

            foreach (var item in inventoryDelta)
            {
                var inventoryItem = inventoryItems[item.Key];
                var quantityChange = item.Value;
                var quantityAfter = inventoryItem.CurrentQuantity;
                var quantityBefore = quantityAfter - quantityChange;


                adjustment.Details.Add(new InventoryAdjustmentDetail
                {
                    InventoryItemId = inventoryItem.InventoryItemId,
                    QuantityBefore = quantityBefore,
                    QuantityAfter = quantityAfter,
                    QuantityChange = quantityChange
                });

                await _unitOfWork.Repository<InventoryAdjustment>().AddAsync(adjustment);
            }
        }
    }
}
