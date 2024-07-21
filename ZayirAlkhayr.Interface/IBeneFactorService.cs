using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;

namespace ZayirAlkhayr.Interface
{
    public interface IBeneFactorService
    {
        DataSet GetAllBeneFactorData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllBeneFactorFilters(PagingFilterModel PagingFilter);
        Task<HandleErrorResponseModel> AddNewBeneFactor(BeneFactors Model);
        Task<HandleErrorResponseModel> UpdateBeneFactor(BeneFactors Model);
        HandleErrorResponseModel DeleteBeneFactor(int BeneFactorId);
        HandleErrorResponseModel AddNewBeneFactorValues(BeneFactorValues Model);
        List<BeneFactorValues> GetAllBeneFactorValuesById(int BeneFactorId);
        DataTable GetAllBeneFactorTypes(PagingFilterModel PagingFilter);
        HandleErrorResponseModel AddNewBeneFactorType(BeneFactorTypes Model);
        DataTable GetAllBeneFactorDetails(PagingFilterModel PagingFilter,int BeneFactorId);
        Task<HandleErrorResponseModel> AddNewBeneFactorDetails(BeneFactorDetails Model);
        DataTable GetAllBeneFactorDetailsByValueId(int BeneFactorValueId);
        string ExportBeneFactorsPDFFile(PDFModel Model, int RowCount);
        string ExportBeneFactorsExcelFile(PDFModel Model, string UserName);
    }
}
