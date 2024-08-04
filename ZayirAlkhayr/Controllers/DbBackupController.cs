using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Service;
using ZayirAlkhayr.Service.Common;

namespace ZayirAlkhayr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbBackupController : ControllerBase
    {
        private readonly IDbBackupService _dbBackupService;
        public DbBackupController(IDbBackupService dbBackupService)
        {
            _dbBackupService = dbBackupService;
        }

        [HttpGet("SaveDbBackupFile")]
        public HandleErrorResponseModel SaveDbBackupFile()
        {
            var results = _dbBackupService.SaveDbBackupFile();
            return results;
        }

        [HttpGet("DownloadImagesFolder")]
        public HandleErrorResponseModel DownloadImagesFolder(ImageFiles Folder)
        {
            var results = _dbBackupService.DownloadImagesFolder(Folder);
            return results;
        }
    }
}
