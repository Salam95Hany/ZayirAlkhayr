using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface.Admin;

namespace ZayirAlkhayr.Service.Admin
{
    public class ManageFileService : IManageFileService
    {
        private readonly IWebHostEnvironment _environment;
        public ManageFileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> UploadFile(IFormFile File, string OldFileName, ImageFiles FolderName)
        {
            string FolderPath = Path.Combine(_environment.WebRootPath, FolderName.ToString());
            if (!string.IsNullOrEmpty(OldFileName))
            {
                DeleteFile(OldFileName, FolderName);
            }
            bool ImageIsExist = CheckFileIsExist(FolderPath, File.FileName);
            if (ImageIsExist)
                throw new Exception("This Image Is Exist Please Upload Another Image");

            var FileName = Guid.NewGuid().ToString() + "_" + File.FileName;

            string extension = Path.GetExtension(File.FileName);
            var supportedTypes = new[] { ".jpg", ".png", ".PNG", ".bmp", ".jpeg", ".jfif", ".webp" };
            if (!supportedTypes.Contains(extension))
            {
                throw new Exception("Uploaded File Extension Not Supported");
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

            return FileName;
        }

        public string DownloadFile(string FileName, ImageFiles FolderName)
        {
            var FolderPath = Path.Combine(_environment.WebRootPath, FolderName.ToString());

            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            string FullPath = Path.Combine(FolderPath, FileName);
            return FullPath;
            //return new TempPhysicalFileResult(FullPath), "application/xlsx");
        }
        public bool DeleteFile(string FileName, ImageFiles FolderName)
        {
            string DirectoryPath = Path.Combine(_environment.WebRootPath, FolderName.ToString());
            string FullPath = Path.Combine(DirectoryPath, FileName);
            if (System.IO.File.Exists(FullPath))
                System.IO.File.Delete(FullPath);

            return true;
        }

        private bool CheckFileIsExist(string FolderPath, string FileName)
        {
            var Files = Directory.GetFiles(FolderPath);
            bool ImageIsExist = Files.Any(i => i.EndsWith(FileName));
            return ImageIsExist;
        }
    }
}
