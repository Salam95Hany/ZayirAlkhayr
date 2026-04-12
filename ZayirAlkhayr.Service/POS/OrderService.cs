using Microsoft.Data.SqlClient;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppSettings _appSettings;
        private readonly ISQLHelper _sQLHelper;
        private string ApiLocalUrl;
        public OrderService(IUnitOfWork unitOfWork, IAppSettings appSettings, ISQLHelper sQLHelper)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings;
            ApiLocalUrl = _appSettings.ApiUrlLocal;
            _sQLHelper = sQLHelper;
        }

        public async Task<ApiResponseModel<DataTable>> GetAllOrders(PagingFilterModel PagingFilter)
        {
            try
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
            catch (Exception ex)
            {

                throw;
            }

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
                    Quantity = i.Quantity,
                    TotalValue = i.Item.Price * i.Quantity,
                }).ToList()
            };

            return ApiResponseModel<OrderDetailsWithCustomer>.Success(GenericErrors.GetSuccess, data);
        }

        public async Task<ApiResponseModel<string>> AddNewOrder(OrderWithDetailsDto Model)
        {
            try
            {
                var Spec = new OrderNumberSpecification(DateTime.Now);
                var OrderNumber = await _unitOfWork.Repository<Order>().MaxAsync(i =>
                i.InsertDate.Value.Year == DateTime.Now.Year
                && i.InsertDate.Value.Month == DateTime.Now.Month
                && i.InsertDate.Value.Day == DateTime.Now.Day
                , i => (int?)i.OrderNumber) ?? 0;
                var order = new Order
                {
                    CustomerId = Model.CustomerId,
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
                return ApiResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<string>> UpdateOrder(OrderWithDetailsDto order)
        {
            try
            {
                var Spec = new OrderDetailsSpecification(order.OrderId.Value);
                var entity = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(Spec);
                if (entity != null)
                {
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
                    entity.UpdateUser = order.UserId;
                    entity.UpdateDate = DateTime.Now;

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

        public async Task<ApiResponseModel<string>> CancelOrder(string VoidReason, string Action, string VoidNotes, int OrderId)
        {
            try
            {
                var Order = await _unitOfWork.Repository<Order>().GetByIdAsync(OrderId);

                if (Order != null)
                {
                    Order.VoidReason = VoidReason;
                    Order.VoidNotes = VoidNotes;
                    Order.OrderStatus = OrderStatus.Cancelled;

                    await _unitOfWork.CompleteAsync();
                    return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
                }

                return ApiResponseModel<string>.Failure(GenericErrors.NotFound);
            }
            catch (Exception Ex)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ApiResponseModel<OrderWithDetailsResponse>> GetOrderWithDetailsByOrderId(int OrderId)
        {
            var Spec = new OrderDetailsSpecification(OrderId);
            var results = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(Spec);
            var data = new OrderWithDetailsResponse();
            if (results != null)
            {
                data.Notes = results.Note;
                data.TotalValue = results.TotalAmount;
                data.OrderType = results.OrderType;
                if (results.Customers != null)
                    data.Customer = new CustomerOrderResponse
                    {
                        CustomerId = results.Customers.CustomerId,
                        FullName = results.Customers.FullName,
                        Phone = results.Customers.Phone,
                        Address = results.Customers.Address
                    };
                data.OrderDetails = results.OrderDetails.Select(i => new OrderDetailsResponse
                {
                    ProductId = i.Item.ItemId,
                    ProductName = i.Item.Name,
                    Image = Path.Combine(ApiLocalUrl, "Images", ImageFiles.Items.ToString(), i.Item.Image ?? string.Empty),
                    Price = i.Item.Price,
                    Quantity = i.Quantity,
                    TotalValue = i.Item.Price * i.Quantity,
                }).ToList();
            }

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
    }
}
