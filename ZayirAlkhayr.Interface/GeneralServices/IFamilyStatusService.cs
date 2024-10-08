using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface.GeneralServices
{
    public interface IFamilyStatusService
    {
        FamilyStatusLookups GetFamilyStatusLookups();
        UpdateFamilyStatusLookups GetUpdateFamilyStatusLookups(int FamilyStatusId);
        DataSet GetAllFamilyStatusData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllFamilyStatusFilter(PagingFilterModel PagingFilter);
        string ExportFamilyStatusDataPDFFile(PDFModel Model, int RowCount);
        string ExportFamilyStatusDataExcelFile(PDFModel Model, string UserName);
    }
}
