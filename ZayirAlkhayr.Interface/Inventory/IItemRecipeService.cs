using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Inventory;

namespace ZayirAlkhayr.Interface.Inventory
{
    public interface IItemRecipeService
    {
        Task<ApiResponseModel<List<ItemRecipeDetailsDto>>> GetAllItemRecipes(PagingFilterModel model);
        Task<ApiResponseModel<ItemRecipeDetailsDto>> GetItemRecipeById(int itemId);
        Task<ApiResponseModel<string>> AddItemRecipe(ItemRecipeUpsertDto model);
        Task<ApiResponseModel<string>> UpdateItemRecipe(ItemRecipeUpsertDto model);
        Task<ApiResponseModel<string>> DeleteItemRecipe(int itemId);
    }
}
