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
using ZayirAlkhayr.Interface.Settings;

namespace ZayirAlkhayr.Service.Settings
{
    public class DbBackupService : IDbBackupService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private string ConnectionString;
        private string ApiLocalUrl;
        public DbBackupService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            ConnectionString = _configuration.GetConnectionString("DBConnection");
            ApiLocalUrl = _configuration["ApiUrlLocal"];
        }
        public string SaveDbBackupFile()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    var backupFilePath = "";
                    string backupQuery = $"BACKUP DATABASE [db6936] TO DISK = '{backupFilePath}'";
                    SqlCommand command = new SqlCommand(backupQuery, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return backupFilePath;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string DownloadImagesFolder(ImageFiles Folder)
        {
            try
            {
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

                    return zipFilePath;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        private string GetBackupImageFilePath(ImageFiles Folder)
        {
            var FullPath = Path.Combine(_environment.WebRootPath, ImageFiles.ExportFiles.ToString());
            var FileName = DateTime.Now.ToString("dd-MM-yyyy") + "_" + Folder.ToString() + ".zip";
            return Path.Combine(FullPath, FileName);
        }
    }

    public class FolderNames
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
