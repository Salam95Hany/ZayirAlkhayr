using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface.Common;

namespace ZayirAlkhayr.Service.Common
{
    public class ManageFileService : IManageFileService
    {
        private readonly IWebHostEnvironment _environment;
        public ManageFileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<ApiResponseModel<string>> UploadFile(IFormFile File, string OldFileName, ImageFiles FolderName)
        {
            string FolderPath = Path.Combine(_environment.WebRootPath, "Images", FolderName.ToString());
            if (!string.IsNullOrEmpty(OldFileName))
            {
                DeleteFile(OldFileName, FolderName);
            }
            bool ImageIsExist = CheckFileIsExist(FolderPath, File.FileName);
            if (ImageIsExist)
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

            var FileName = Guid.NewGuid().ToString() + "_" + File.FileName;

            string extension = Path.GetExtension(File.FileName);
            var supportedTypes = new[] { ".jpg", ".JPG", ".png", ".PNG", ".bmp", ".jpeg", ".JPEG", ".jfif", ".webp" };
            if (!supportedTypes.Contains(extension))
            {
                return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
            else
            {
                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);

                if (File.Length > 0)
                {
                    using (var stream = new FileStream(Path.Combine(FolderPath, FileName), FileMode.Create))
                    {
                        await File.CopyToAsync(stream);
                    }
                }
            }
            return ApiResponseModel<string>.Success(GenericErrors.AddSuccess, FileName);
        }

        public ApiResponseModel<string> DeleteFile(string FileName, ImageFiles FolderName)
        {
            string DirectoryPath = Path.Combine(_environment.WebRootPath, "Images", FolderName.ToString());
            string FullPath = Path.Combine(DirectoryPath, FileName);
            if (File.Exists(FullPath))
            {
                try
                {
                    File.Delete(FullPath);
                }
                catch (Exception)
                {
                    return ApiResponseModel<string>.Failure(GenericErrors.TransFailed);
                }

            }

            return ApiResponseModel<string>.Success(GenericErrors.DeleteSuccess);
        }

        private bool CheckFileIsExist(string FolderPath, string FileName)
        {
            var Files = Directory.GetFiles(FolderPath);
            bool ImageIsExist = Files.Any(i => i.EndsWith(FileName));
            return ImageIsExist;
        }
    }
}
