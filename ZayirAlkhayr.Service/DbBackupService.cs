using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PosSystem.Entities.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface;

namespace ZayirAlkhayr.Service
{
    public class DbBackupService : IDbBackupService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private string ConnectionString;
        private string BackupFilePath;
        private string ImageFilePath;
        public DbBackupService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            ConnectionString = _configuration.GetConnectionString("DBConnection");
            BackupFilePath = _configuration["BackupFilePath"];
            ImageFilePath = _configuration["ImageFilePath"];
        }
        public HandleErrorResponseModel SaveDbBackupFile()
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    var backupFilePath = GetBackupFilePath();
                    string backupQuery = $"BACKUP DATABASE [ZayirAlkhayrDB] TO DISK = '{backupFilePath}'";
                    SqlCommand command = new SqlCommand(backupQuery, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    Response.Done = true;
                    Response.Message = "تم أخذ نسخة احتياطية بنجاح";
                    return Response;
                }
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel DownloadImagesFolder(ImageFiles Folder)
        {
            try
            {
                var Response = new HandleErrorResponseModel();
                string folderPath = Path.Combine(_environment.WebRootPath, Folder.ToString());
                string zipFilePath = GetBackupImageFilePath(Folder);
                using (var zipArchive = new ZipArchive(File.Create(zipFilePath), ZipArchiveMode.Create))
                {
                    foreach (var filePath in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
                    {
                        var relativePath = filePath.Substring(folderPath.Length + 1);
                        var zipEntry = zipArchive.CreateEntry(relativePath);

                        using (var sourceStream = File.OpenRead(filePath))
                        using (var entryStream = zipEntry.Open())
                        {
                            sourceStream.CopyTo(entryStream);
                        }
                    }

                    Response.Done = true;
                    Response.Message = "تم أخذ نسخة احتياطية بنجاح";
                    return Response;
                }
            }
            catch (Exception)
            {
                var Response = new HandleErrorResponseModel();
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }

        }

        private string GetBackupFilePath()
        {
            if (!Directory.Exists(BackupFilePath))
                Directory.CreateDirectory(BackupFilePath);

            var FullPath = Path.Combine(BackupFilePath, "ZADBbk");
            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);

            var FileName = DateTime.Now.ToString("dd-MM-yyyy") + "_ZAbk.bak";

            return Path.Combine(FullPath, FileName);
        }

        private string GetBackupImageFilePath(ImageFiles Folder)
        {

            var FolderNames = new List<FolderNames>
            {
                new FolderNames { NameEn = "ActivityImages", NameAr = "الأنشطة" },
                new FolderNames { NameEn = "ActivitySliderImages", NameAr = "تفاصيل الأنشطة" },
                new FolderNames { NameEn = "BeneFactorDetailsImages", NameAr = "تفاصيل المتبرعين" },
                new FolderNames { NameEn = "BeneFactorImages", NameAr = "المتبرعين" },
                new FolderNames { NameEn = "EventSliderImages", NameAr = "الفعاليات" },
                new FolderNames { NameEn = "PhotoDetailImages", NameAr = "تفاصيل الصور" },
                new FolderNames { NameEn = "PhotoImages", NameAr = "الصور" },
                new FolderNames { NameEn = "SliderImages", NameAr = "شريط الصور" }
            };


            if (!Directory.Exists(ImageFilePath))
                Directory.CreateDirectory(ImageFilePath);

            var FullPath = Path.Combine(ImageFilePath, "ZADBbk");
            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);

            var FolderNameAr = FolderNames.FirstOrDefault(i => i.NameEn == Folder.ToString()).NameAr;
            var FullFolderName = DateTime.Now.ToString("dd-MM-yyyy") + "_" + FolderNameAr + ".zip";

            return Path.Combine(FullPath, FullFolderName);
        }
    }

    public class FolderNames
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
