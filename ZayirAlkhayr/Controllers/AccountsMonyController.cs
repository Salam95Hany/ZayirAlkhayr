using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Service;
using ZayirAlkhayr.Service.Common;

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

        [HttpPost("GetAllAccountsImportMonyData")]
        public DataTable GetAllAccountsImportMonyData(PagingFilterModel PagingFilter)
        {
            var results = _accountsMonyService.GetAllAccountsImportMonyData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllAccountsImportMonyFilters")]
        public List<FilterModel> GetAllAccountsImportMonyFilters(PagingFilterModel PagingFilter)
        {
            var results = _accountsMonyService.GetAllAccountsImportMonyFilters(PagingFilter);
            return results;
        }

        [HttpPost("GetAllAccountsExportMonyData")]
        public DataTable GetAllAccountsExportMonyData(PagingFilterModel PagingFilter)
        {
            var results = _accountsMonyService.GetAllAccountsExportMonyData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllAccountsExportMonyFilters")]
        public List<FilterModel> GetAllAccountsExportMonyFilters(PagingFilterModel PagingFilter)
        {
            var results = _accountsMonyService.GetAllAccountsExportMonyFilters(PagingFilter);
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

        [HttpPost("UpdateAccountsImportMony")]
        public HandleErrorResponseModel UpdateAccountsImportMony(AccountsImportMony Model)
        {
            var results = _accountsMonyService.UpdateAccountsImportMony(Model);
            return results;
        }

        [HttpGet("DeleteAccountsImportMony")]
        public HandleErrorResponseModel DeleteAccountsImportMony(int AccountId)
        {
            var results = _accountsMonyService.DeleteAccountsImportMony(AccountId);
            return results;
        }

        [HttpPost("AddNewAccountsExportMony")]
        public HandleErrorResponseModel AddNewAccountsExportMony(AccountsExportMony Model)
        {
            var results = _accountsMonyService.AddNewAccountsExportMony(Model);
            return results;
        }

        [HttpPost("UpdateAccountsExportMony")]
        public HandleErrorResponseModel UpdateAccountsExportMony(AccountsExportMony Model)
        {
            var results = _accountsMonyService.UpdateAccountsExportMony(Model);
            return results;
        }

        [HttpGet("DeleteAccountsExportMony")]
        public HandleErrorResponseModel DeleteAccountsExportMony(int AccountId)
        {
            var results = _accountsMonyService.DeleteAccountsExportMony(AccountId);
            return results;
        }

        [HttpPost("ExportAccountsImportMonyExcelFile")]
        public IActionResult ExportAccountsImportMonyExcelFile(PDFModel Model, string UserName)
        {
            var FullPath = _accountsMonyService.ExportAccountsImportMonyExcelFile(Model, UserName);
            return new TempPhysicalFileResult(FullPath, "application/xlsx");
        }

        [HttpPost("ExportAccountsExportMonyExcelFile")]
        public IActionResult ExportAccountsExportMonyExcelFile(PDFModel Model, string UserName)
        {
            var FullPath = _accountsMonyService.ExportAccountsExportMonyExcelFile(Model, UserName);
            return new TempPhysicalFileResult(FullPath, "application/xlsx");
        }
    }
}
