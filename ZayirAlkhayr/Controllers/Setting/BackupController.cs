using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using ZayirAlkhayr.Interface.Setting;

namespace ZayirAlkhayr.Controllers.Setting
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly IBackupService _backupService;

        public BackupController(IBackupService backupService)
        {
            _backupService = backupService;
        }

        [HttpGet("DownloadImagesBackup")]
        public async Task<IActionResult> DownloadImagesBackup(string backupType = "all")
        {
            try
            {
                var archive = await _backupService.CreateImagesBackupAsync(backupType);
                return File(archive.FileContents, archive.ContentType, archive.FileName);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
