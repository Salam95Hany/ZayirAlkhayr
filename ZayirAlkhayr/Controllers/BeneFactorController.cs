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

        [HttpGet("GetAllBeneFactorValuesById")]
        public List<BeneFactorValues> GetAllBeneFactorValuesById(int BeneFactorId)
        {
            var results = _beneFactorService.GetAllBeneFactorValuesById(BeneFactorId);
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

        [HttpGet("GetAllBeneFactorDetailsByValueId")]
        public DataTable GetAllBeneFactorDetailsByValueId(int BeneFactorValueId)
        {
            var results = _beneFactorService.GetAllBeneFactorDetailsByValueId(BeneFactorValueId);
            return results;
        }

        [HttpPost("AddNewBeneFactor")]
        public async Task<HandleErrorResponseModel> AddNewBeneFactor([FromForm] BeneFactors Model)
        {
            var results = await _beneFactorService.AddNewBeneFactor(Model);
            return results;
        }

        [HttpPost("AddNewBeneFactorValues")]
        public HandleErrorResponseModel AddNewBeneFactorValues(BeneFactorValues Model)
        {
            var results = _beneFactorService.AddNewBeneFactorValues(Model);
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
