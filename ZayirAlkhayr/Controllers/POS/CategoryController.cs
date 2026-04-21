using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.POS;

namespace ZayirAlkhayr.Controllers.POS
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("GetAllCategories")]
        public async Task<ApiResponseModel<List<Category>>> GetAll(PagingFilterModel Model)
        {
            var results = await _categoryService.GetAllCategories(Model);
            return results;
        }

        [HttpPost("AddNewCategory")]
        public async Task<ApiResponseModel<string>> AddNewCategory([FromForm] Category Model)
        {
            var results = await _categoryService.AddNewCategory(Model);
            return results;

        }

        [HttpPost("UpdateCategory")]
        public async Task<ApiResponseModel<string>> UpdateCategory([FromForm] Category Model)
        {
            var results = await _categoryService.UpdateCategory(Model);
            return results;

        }

        [HttpGet("DeleteCategory")]
        public async Task<ApiResponseModel<string>> DeleteCategory(int CategoryId)
        {
            var results = await _categoryService.DeleteCategory(CategoryId);
            return results;
        }
    }
}
