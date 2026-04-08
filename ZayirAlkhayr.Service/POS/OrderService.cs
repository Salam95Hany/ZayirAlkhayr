using System;
using System.Collections.Generic;
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
        private string ApiLocalUrl;
        public OrderService(IUnitOfWork unitOfWork, IAppSettings appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings;
            ApiLocalUrl = _appSettings.ApiUrlLocal;
        }

        public async Task<ApiResponseModel<List<Order>>> GetAllOrders(PagingFilterModel Model)
        {
            var SearchText = Model.FilterList.FirstOrDefault(i => i.CategoryName == "SearchText")?.ItemId;
            var Spec = new OrderSpecification(SearchText);
            var results = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            var Count = await _unitOfWork.Repository<Order>().CountAsync();
            return ApiResponseModel<List<Order>>.Success(GenericErrors.GetSuccess, results, Count);
        }

        public async Task<ApiResponseModel<List<OrderDetailsResponse>>> GetOrderDetailsByOrderId(int OrderId)
        {
            var Spec = new OrderDetailsSpecification(OrderId);
            var results = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(Spec);
            var data = results.OrderDetails.Select(i => new OrderDetailsResponse
            {
                ProductId = i.Item.ItemId,
                ProductName = i.Item.Name,
                CategoryName = i.Item.Category.Name,
                Image = Path.Combine(ApiLocalUrl, "Images", ImageFiles.Items.ToString(), i.Item.Image ?? string.Empty),
                Price = i.Item.Price,
                Quantity = i.Quantity,
                TotalValue = i.Item.Price * i.Quantity,
            }).ToList();
            return ApiResponseModel<List<OrderDetailsResponse>>.Success(GenericErrors.GetSuccess, data);
        }

        public async Task<ApiResponseModel<string>> AddNewOrder(OrderWithDetailsDto dto)
        {
            try
            {
                var Spec = new OrderNumberSpecification(DateTime.UtcNow);
                var OrderNumber = await _unitOfWork.Repository<Order>().MaxAsync(i =>
                i.InsertDate.Value.Year == DateTime.UtcNow.Year
                && i.InsertDate.Value.Month == DateTime.UtcNow.Month
                && i.InsertDate.Value.Day == DateTime.UtcNow.Day
                , i => (int?)i.OrderNumber) ?? 0;
                var order = new Order
                {
                    CustomerId = dto.CustomerID,
                    TotalAmount = dto.TotalAmount,
                    OrderStatus = OrderStatus.Completed,
                    Note = dto.Note,
                    InsertUser = dto.UserId,
                    InsertDate = DateTime.UtcNow,
                    OrderNumber = OrderNumber + 1
                };

                await _unitOfWork.Repository<Order>().AddAsync(order);
                await _unitOfWork.CompleteAsync();

                foreach (var detail in dto.Details)
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
                    entity.UpdateDate = DateTime.UtcNow;

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
    }
}
