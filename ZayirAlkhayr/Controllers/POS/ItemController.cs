using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.POS;

namespace ZayirAlkhayr.Controllers.POS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost("GetAllProducts")]
        public async Task<ApiResponseModel<List<ProductsResponseDto>>> GetAllProducts(PagingFilterModel Model)
        {
            var results = await _itemService.GetAllProducts(Model);
            return results;
        }

        [HttpPost("AddNewProduct")]
        public async Task<ApiResponseModel<string>> AddNewProduct([FromForm] Item Model)
        {
            var results = await _itemService.AddNewProduct(Model);
            return results;
        }

        [HttpPost("UpdateProduct")]
        public async Task<ApiResponseModel<string>> UpdateProduct([FromForm] Item Model)
        {
            var results = await _itemService.UpdateProduct(Model);
            return results;

        }

        [HttpGet("DeleteProduct")]
        public async Task<ApiResponseModel<string>> DeleteProduct(int ProductId)
        {
            var results = await _itemService.DeleteProduct(ProductId);
            return results;

        }

        [HttpGet("GetProductsByCategoryId")]
        public async Task<ApiResponseModel<List<Item>>> GetProductsByCategoryId(int CategoryId)
        {
            var results = await _itemService.GetProductsByCategoryId(CategoryId);
            return results;
        }
    }
}
