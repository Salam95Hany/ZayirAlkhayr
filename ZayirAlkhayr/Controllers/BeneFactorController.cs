using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneFactorController : ControllerBase
    {
        private readonly IBeneFactorService _beneFactorService;
        public BeneFactorController(IBeneFactorService beneFactorService)
        {
            _beneFactorService = beneFactorService;
        }

        [HttpGet("BeneFactorLogin")]
        public BeneFactorLoginModel BeneFactorLogin(int Code, string BeneFactorName)
        {
            var results = _beneFactorService.BeneFactorLogin(Code, BeneFactorName);
            return results;
        }

        [HttpPost("GetAllBeneFactorData")]
        public DataSet GetAllBeneFactorData(PagingFilterModel PagingFilter)
        {
            var results = _beneFactorService.GetAllBeneFactorData(PagingFilter);
            return results;
        }

        [HttpPost("GetAllBeneFactorFilters")]
        public List<FilterModel> GetAllBeneFactorFilters(PagingFilterModel PagingFilter)
        {
            var results = _beneFactorService.GetAllBeneFactorFilters(PagingFilter);
            return results;
        }

        [HttpPost("GetAllBeneFactorTypes")]
        public DataTable GetAllBeneFactorTypes(PagingFilterModel PagingFilter)
        {
            var results = _beneFactorService.GetAllBeneFactorTypes(PagingFilter);
            return results;
        }

        [HttpPost("GetAllBeneFactorDetails")]
        public DataTable GetAllBeneFactorDetails(PagingFilterModel PagingFilter, int BeneFactorId)
        {
            var results = _beneFactorService.GetAllBeneFactorDetails(PagingFilter, BeneFactorId);
            return results;
        }

        [HttpGet("GetAllBeneFactorCashDetails")]
        public DataTable GetAllBeneFactorCashDetails(int BeneFactorId, int ParentId)
        {
            var results = _beneFactorService.GetAllBeneFactorCashDetails(BeneFactorId, ParentId);
            return results;
        }

        [HttpGet("GetBeneFactorDetailsByBeneFactorId")]
        public DataTable GetBeneFactorDetailsByBeneFactorId(int BeneFactorId, int BeneFactorTypeId)
        {
            var results = _beneFactorService.GetBeneFactorDetailsByBeneFactorId(BeneFactorId, BeneFactorTypeId);
            return results;
        }

        [HttpGet("GetBeneFactorDetailsStatistics")]
        public DataTable GetBeneFactorDetailsStatistics(int BeneFactorId)
        {
            var results = _beneFactorService.GetBeneFactorDetailsStatistics(BeneFactorId);
            return results;
        }

        [HttpPost("GetBeneFactorTypeByIds")]
        public List<BeneFactorTypes> GetBeneFactorTypeByIds(List<int> Ids)
        {
            var results = _beneFactorService.GetBeneFactorTypeByIds(Ids);
            return results;
        }

        [HttpGet("GetAllBeneFactorParentById")]
        public List<BeneFactorDetails> GetAllBeneFactorParentById(int BeneFactorId)
        {
            var results = _beneFactorService.GetAllBeneFactorParentById(BeneFactorId);
            return results;
        }

        [HttpPost("GetBeneFactorNotes")]
        public DataTable GetBeneFactorNotes(PagingFilterModel PagingFilter)
        {
            var results = _beneFactorService.GetBeneFactorNotes(PagingFilter);
            return results;
        }

        [HttpGet("GetBeneFactorWelcomeMessage")]
        public BeneFactorWelcomeMessage GetBeneFactorWelcomeMessage()
        {
            var results = _beneFactorService.GetBeneFactorWelcomeMessage();
            return results;
        }

        [HttpPost("AddNewBeneFactor")]
        public async Task<HandleErrorResponseModel> AddNewBeneFactor([FromForm] BeneFactors Model)
        {
            var results = await _beneFactorService.AddNewBeneFactor(Model);
            return results;
        }

        [HttpPost("AddNewBeneFactorType")]
        public HandleErrorResponseModel AddNewBeneFactorType(BeneFactorTypes Model)
        {
            var results = _beneFactorService.AddNewBeneFactorType(Model);
            return results;
        }

        [HttpPost("AddNewBeneFactorDetails")]
        public async Task<HandleErrorResponseModel> AddNewBeneFactorDetails([FromForm] BeneFactorDetails Model)
        {
            var results = await _beneFactorService.AddNewBeneFactorDetails(Model);
            return results;
        }

        [HttpPost("AddNewBeneFactorNotes")]
        public HandleErrorResponseModel AddNewBeneFactorNotes(BeneFactorNotes Model)
        {
            var results = _beneFactorService.AddNewBeneFactorNotes(Model);
            return results;
        }

        [HttpPost("AddNewBeneFactorWelcomeMessage")]
        public HandleErrorResponseModel AddNewBeneFactorWelcomeMessage(BeneFactorWelcomeMessage Model)
        {
            var results = _beneFactorService.AddNewBeneFactorWelcomeMessage(Model);
            return results;
        }

        [HttpPost("UpdateBeneFactor")]
        public async Task<HandleErrorResponseModel> UpdateBeneFactor([FromForm] BeneFactors Model)
        {
            var results = await _beneFactorService.UpdateBeneFactor(Model);
            return results;
        }

        [HttpGet("DeleteBeneFactor")]
        public HandleErrorResponseModel DeleteBeneFactor(int BeneFactorId)
        {
            var results = _beneFactorService.DeleteBeneFactor(BeneFactorId);
            return results;
        }

        [HttpPost("ExportBeneFactorsPDFFile")]
        public IActionResult ExportBeneFactorsPDFFile(PDFModel Model, int RowCount)
        {
            var FullPath = _beneFactorService.ExportBeneFactorsPDFFile(Model, RowCount);
            return new TempPhysicalFileResult(FullPath, "application/pdf");
        }

        [HttpPost("ExportBeneFactorsExcelFile")]
        public IActionResult ExportBeneFactorsExcelFile(PDFModel Model, string UserName)
        {
            var FullPath = _beneFactorService.ExportBeneFactorsExcelFile(Model, UserName);
            return new TempPhysicalFileResult(FullPath, "application/xlsx");
        }
    }
}
