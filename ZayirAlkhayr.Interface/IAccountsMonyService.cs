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
        DataTable GetAllAccountsImportMony(PagingFilterModel PagingFilter);
        DataTable GetAllAccountsExportMony(PagingFilterModel PagingFilter);
        DataTable GetAllImportExportMonyStatistics(PagingFilterModel PagingFilter);
        HandleErrorResponseModel AddNewAccountsImportMony(AccountsImportMony Model);
        HandleErrorResponseModel AddNewAccountsExportMony(AccountsExportMony Model);
    }
}
