using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;


namespace ZayirAlkhayr.Interface.BeneFactor
{
    public interface IBeneFactorService
    {
        BeneFactorLoginModel BeneFactorLogin(int Code, string BeneFactorName);
        DataSet GetAllBeneFactorData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllBeneFactorFilters(PagingFilterModel PagingFilter);
        Task<HandleErrorResponseModel> AddNewBeneFactor(BeneFactors Model);
        Task<HandleErrorResponseModel> UpdateBeneFactor(BeneFactors Model);
        HandleErrorResponseModel DeleteBeneFactor(int BeneFactorId);
        DataTable GetAllBeneFactorTypes(PagingFilterModel PagingFilter);
        HandleErrorResponseModel AddNewBeneFactorType(BeneFactorTypes Model);
        DataTable GetAllBeneFactorDetails(PagingFilterModel PagingFilter, int BeneFactorId);
        Task<HandleErrorResponseModel> AddNewBeneFactorDetails(BeneFactorDetails Model);
        DataTable GetAllBeneFactorCashDetails(int BeneFactorId, int ParentId);
        DataTable GetBeneFactorDetailsByBeneFactorId(int BeneFactorId, int BeneFactorTypeId);
        DataTable GetBeneFactorDetailsStatistics(int BeneFactorId);
        List<BeneFactorTypes> GetBeneFactorTypeByIds(List<int> Ids);
        List<BeneFactorDetails> GetAllBeneFactorParentById(int BeneFactorId);
        DataTable GetBeneFactorNotes(PagingFilterModel PagingFilter);
        DataTable GetAllBeneFactorNationalities(PagingFilterModel PagingFilter);
        HandleErrorResponseModel AddNewBeneFactorNotes(BeneFactorNotes Model);
        HandleErrorResponseModel AddNewBeneFactorNationality(BeneFactorNationalities Model);
        string ExportBeneFactorsPDFFile(PDFModel Model, int RowCount);
        string ExportBeneFactorsExcelFile(PDFModel Model, string UserName);
    }
}
