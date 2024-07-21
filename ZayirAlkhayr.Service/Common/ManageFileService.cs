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
        public async Task<HandleErrorResponseModel> UploadFile(IFormFile File, string OldFileName, ImageFiles FolderName)
        {
            var Response = new HandleErrorResponseModel();
            string FolderPath = Path.Combine(_environment.WebRootPath, FolderName.ToString());
            if (!string.IsNullOrEmpty(OldFileName))
            {
                DeleteFile(OldFileName, FolderName);
            }
            bool ImageIsExist = CheckFileIsExist(FolderPath, File.FileName);
            if (ImageIsExist)
            {
                Response.Done = false;
                Response.Message = "الصورة موجودة برجاء ادخال صورة اخرى";
                return Response;
            }

            var FileName = Guid.NewGuid().ToString() + "_" + File.FileName;

            string extension = Path.GetExtension(File.FileName);
            var supportedTypes = new[] { ".jpg", ".png", ".PNG", ".bmp", ".jpeg", ".jfif", ".webp" };
            if (!supportedTypes.Contains(extension))
            {
                Response.Done = false;
                Response.Message = "صيغة الملف غير مقبولة";
                return Response;
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

            Response.Done = true;
            Response.Message = "تمت اضافة الصورة بنجاح";
            Response.StringValue = FileName;
            return Response;
        }

        public HandleErrorResponseModel DeleteFile(string FileName, ImageFiles FolderName)
        {
            var Response = new HandleErrorResponseModel();
            string DirectoryPath = Path.Combine(_environment.WebRootPath, FolderName.ToString());
            string FullPath = Path.Combine(DirectoryPath, FileName);
            if (File.Exists(FullPath))
            {
                try
                {
                    File.Delete(FullPath);
                }
                catch (Exception)
                {
                    Response.Done = false;
                    Response.Message = "لقد حدث خطا لا يمكن حذف هذه الصورة";
                }
                
            }

            Response.Done = true;
            Response.Message = "تم حذف الصورة بنجاح";
            return Response;
        }

        private bool CheckFileIsExist(string FolderPath, string FileName)
        {
            var Files = Directory.GetFiles(FolderPath);
            bool ImageIsExist = Files.Any(i => i.EndsWith(FileName));
            return ImageIsExist;
        }
    }
}
