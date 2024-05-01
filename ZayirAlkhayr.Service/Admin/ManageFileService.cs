using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface.Admin;

namespace ZayirAlkhayr.Service.Admin
{
    public class ManageFileService: IManageFileService
    {
        private readonly IWebHostEnvironment _environment;
        public ManageFileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public string UploadImageFile() //[FromForm] UploadFileModel model
        {
            //bool ImageIsExist = CheckImageIsExist(model.File.FileName);
            //if (ImageIsExist)
            //    return "This Image Is Exist Please Upload Another Image";

            var FileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
            string directoryPath = Path.Combine(_environment.WebRootPath, ImageFiles.SliderImages.ToString());
            //string extension = System.IO.Path.GetExtension(model.File.FileName);
            //var supportedTypes = new[] { ".jpg", ".png", ".PNG", ".bmp", ".jpeg", ".jfif", ".webp" };
            //if (!supportedTypes.Contains(extension))
            //{
            //    return "Uploaded File Extension Not Supported";
            //}
            //else
            //{
            //    if (!System.IO.Directory.Exists(directoryPath))
            //    {
            //        System.IO.Directory.CreateDirectory(directoryPath);
            //    }

            //    if (model.File.Length > 0)
            //    {
            //        using (var stream = new FileStream(Path.Combine(directoryPath, FileName), FileMode.Create))
            //        {
            //            await model.File.CopyToAsync(stream);
            //        }
            //    }
            //}

            return FileName;

            //return new TempPhysicalFileResult(Path.Combine(_environment.WebRootPath, "PartSummary_20240407_20240408.xlsx"), "application/xlsx");
        }

        public string DeleteImageFile(string FileName)
        {
            string DirectoryPath = Path.Combine(_environment.WebRootPath, ImageFiles.SliderImages.ToString());
            string FullPath = Path.Combine(DirectoryPath, FileName);
            if (System.IO.File.Exists(FullPath))
                System.IO.File.Delete(FullPath);

            return "File Deleted Successfully";
        }

        private bool CheckImageIsExist(string FolderPath, string FileName)
        {
            var Files = Directory.GetFiles(FolderPath);
            bool ImageIsExist = Files.Any(i => i.EndsWith(FileName));
            return ImageIsExist;
        }
    }
}
