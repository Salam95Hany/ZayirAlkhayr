using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.POS
{
    public interface ICategoryService
    {
        Task<ApiResponseModel<List<Category>>> GetAllCategories(PagingFilterModel Model);
        Task<ApiResponseModel<string>> AddNewCategory(Category Model);
        Task<ApiResponseModel<string>> UpdateCategory(Category Model);
        Task<ApiResponseModel<string>> DeleteCategory(int CategoryId);
    }
}
