using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.POS
{
    public interface IItemService
    {
        Task<ApiResponseModel<List<ProductsResponseDto>>> GetAllProducts(PagingFilterModel Model);
        Task<ApiResponseModel<string>> AddNewProduct(Item Model);
        Task<ApiResponseModel<string>> UpdateProduct(Item Model);
        Task<ApiResponseModel<string>> DeleteProduct(int ProductId);
        Task<ApiResponseModel<List<Item>>> GetProductsByCategoryId(int CategoryId);
    }
}
