using System.Collections.Generic;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface.Inventory
{
    public interface ISupplierService
    {
        Task<ApiResponseModel<List<Supplier>>> GetAllSuppliers(PagingFilterModel model);
        Task<ApiResponseModel<Supplier>> GetSupplierById(int supplierId);
        Task<ApiResponseModel<string>> AddNewSupplier(Supplier model);
        Task<ApiResponseModel<string>> UpdateSupplier(Supplier model);
        Task<ApiResponseModel<string>> DeleteSupplier(int supplierId);
    }
}
