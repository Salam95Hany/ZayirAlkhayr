using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface.Admin
{
    public interface IManageFileService
    {
        Task<string> UploadFile(IFormFile File, string OldFileName, ImageFiles FolderName);
        string DownloadFile(string FileName, ImageFiles FolderName);
        bool DeleteFile(string FileName, ImageFiles FolderName);
    }
}
