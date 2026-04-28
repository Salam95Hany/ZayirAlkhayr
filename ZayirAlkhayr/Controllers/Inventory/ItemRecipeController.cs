using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;
using ZayirAlkhayr.Interface.Inventory;

namespace ZayirAlkhayr.Controllers.Inventory
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemRecipeController : ControllerBase
    {
        private readonly IItemRecipeService _itemRecipeService;

        public ItemRecipeController(IItemRecipeService itemRecipeService)
        {
            _itemRecipeService = itemRecipeService;
        }

        [HttpPost("GetAllItemRecipes")]
        public async Task<ApiResponseModel<List<ItemRecipeDetailsDto>>> GetAllItemRecipes(PagingFilterModel model)
        {
            return await _itemRecipeService.GetAllItemRecipes(model);
        }

        [HttpGet("GetItemRecipeById")]
        public async Task<ApiResponseModel<ItemRecipeDetailsDto>> GetItemRecipeById(int itemId)
        {
            return await _itemRecipeService.GetItemRecipeById(itemId);
        }

        [HttpPost("AddItemRecipe")]
        public async Task<ApiResponseModel<string>> AddItemRecipe(ItemRecipeUpsertDto model)
        {
            return await _itemRecipeService.AddItemRecipe(model);
        }

        [HttpPost("UpdateItemRecipe")]
        public async Task<ApiResponseModel<string>> UpdateItemRecipe(ItemRecipeUpsertDto model)
        {
            return await _itemRecipeService.UpdateItemRecipe(model);
        }

        [HttpGet("DeleteItemRecipe")]
        public async Task<ApiResponseModel<string>> DeleteItemRecipe(int itemId)
        {
            return await _itemRecipeService.DeleteItemRecipe(itemId);
        }
    }
}
