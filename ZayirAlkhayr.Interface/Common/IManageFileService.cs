using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface.Common
{
    public interface IManageFileService
    {
        Task<HandleErrorResponseModel> UploadFile(IFormFile File, string OldFileName, ImageFiles FolderName);
        HandleErrorResponseModel DeleteFile(string FileName, ImageFiles FolderName);
    }
}
