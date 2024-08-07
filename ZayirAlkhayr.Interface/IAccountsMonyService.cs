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
    public interface IAccountsMonyService
    {
        DataTable GetAllAccountsImportMonyData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllAccountsImportMonyFilters(PagingFilterModel PagingFilter);
        DataTable GetAllAccountsExportMonyData(PagingFilterModel PagingFilter);
        List<FilterModel> GetAllAccountsExportMonyFilters(PagingFilterModel PagingFilter);
        DataTable GetAllImportExportMonyStatistics(PagingFilterModel PagingFilter);
        HandleErrorResponseModel AddNewAccountsImportMony(AccountsImportMony Model);
        HandleErrorResponseModel UpdateAccountsImportMony(AccountsImportMony Model);
        HandleErrorResponseModel DeleteAccountsImportMony(int AccountId);
        HandleErrorResponseModel AddNewAccountsExportMony(AccountsExportMony Model);
        HandleErrorResponseModel UpdateAccountsExportMony(AccountsExportMony Model);
        HandleErrorResponseModel DeleteAccountsExportMony(int AccountId);
        string ExportAccountsImportMonyExcelFile(PDFModel Model, string UserName);
        string ExportAccountsExportMonyExcelFile(PDFModel Model, string UserName);
    }
}
