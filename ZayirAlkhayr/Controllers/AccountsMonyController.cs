using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Service;

namespace ZayirAlkhayr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsMonyController : ControllerBase
    {
        private readonly IAccountsMonyService _accountsMonyService;
        public AccountsMonyController(IAccountsMonyService accountsMonyService)
        {
            _accountsMonyService = accountsMonyService;
        }

        [HttpPost("GetAllAccountsImportMony")]
        public DataTable GetAllAccountsImportMony(PagingFilterModel PagingFilter)
        {
            var results = _accountsMonyService.GetAllAccountsImportMony(PagingFilter);
            return results;
        }

        [HttpPost("GetAllAccountsExportMony")]
        public DataTable GetAllAccountsExportMony(PagingFilterModel PagingFilter)
        {
            var results = _accountsMonyService.GetAllAccountsExportMony(PagingFilter);
            return results;
        }

        [HttpPost("GetAllImportExportMonyStatistics")]
        public DataTable GetAllImportExportMonyStatistics(PagingFilterModel PagingFilter)
        {
            var results = _accountsMonyService.GetAllImportExportMonyStatistics(PagingFilter);
            return results;
        }

        [HttpPost("AddNewAccountsImportMony")]
        public HandleErrorResponseModel AddNewAccountsImportMony(AccountsImportMony Model)
        {
            var results = _accountsMonyService.AddNewAccountsImportMony(Model);
            return results;
        }

        [HttpPost("AddNewAccountsExportMony")]
        public HandleErrorResponseModel AddNewAccountsExportMony(AccountsExportMony Model)
        {
            var results = _accountsMonyService.AddNewAccountsExportMony(Model);
            return results;
        }
    }
}
