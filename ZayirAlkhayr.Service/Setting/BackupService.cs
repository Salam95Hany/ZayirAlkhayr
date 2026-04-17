using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Interface.Setting;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.IO.Compression;
using ZayirAlkhayr.Entities.Contracts.DTOs.Setting;

namespace ZayirAlkhayr.Service.Setting
{
    public class BackupService : IBackupService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BackupService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<BackupArchiveDto> CreateImagesBackupAsync(string backupType)
        {
            var normalizedType = NormalizeBackupType(backupType);
            var webRootPath = ResolveWebRootPath();

            var selectedDirectories = GetSelectedDirectories(webRootPath, normalizedType).ToList();
            var existingDirectories = selectedDirectories.Where(i => Directory.Exists(i.PhysicalPath)).ToList();

            if (existingDirectories.Count == 0)
                throw new DirectoryNotFoundException("لم يتم العثور على أي مجلد صور مطابق للنسخة المطلوبة.");

            await using var memoryStream = new MemoryStream();

            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var directory in existingDirectories)
                    AddDirectoryToArchive(archive, directory.PhysicalPath, directory.EntryPrefix);
            }

            return new BackupArchiveDto
            {
                FileContents = memoryStream.ToArray(),
                FileName = BuildFileName(normalizedType)
            };
        }

        private string ResolveWebRootPath()
        {
            if (!string.IsNullOrWhiteSpace(_webHostEnvironment.WebRootPath))
                return _webHostEnvironment.WebRootPath;

            return Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
        }

        private static string NormalizeBackupType(string backupType)
        {
            var normalizedType = (backupType ?? "all").Trim().ToLowerInvariant();
            if (normalizedType == "both")
                normalizedType = "all";

            if (normalizedType != "items" && normalizedType != "categories" && normalizedType != "all")
                throw new ArgumentException("قيمة backupType يجب أن تكون items أو categories أو all.");

            return normalizedType;
        }

        private static IEnumerable<(string PhysicalPath, string EntryPrefix)> GetSelectedDirectories(string webRootPath, string backupType)
        {
            var itemsPath = Path.Combine(webRootPath, "Images", "Items");
            var categoriesPath = Path.Combine(webRootPath, "Images", "Categories");

            if (backupType == "items")
            {
                yield return (itemsPath, "Images/Items");
                yield break;
            }

            if (backupType == "categories")
            {
                yield return (categoriesPath, "Images/Categories");
                yield break;
            }

            yield return (itemsPath, "Images/Items");
            yield return (categoriesPath, "Images/Categories");
        }

        private static void AddDirectoryToArchive(ZipArchive archive, string sourceDirectory, string entryPrefix)
        {
            var files = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                archive.CreateEntry($"{entryPrefix.TrimEnd('/')}/");
                return;
            }

            foreach (var filePath in files)
            {
                var relativePath = Path.GetRelativePath(sourceDirectory, filePath).Replace("\\", "/");
                archive.CreateEntryFromFile(filePath, $"{entryPrefix.TrimEnd('/')}/{relativePath}", CompressionLevel.Fastest);
            }
        }

        private static string BuildFileName(string backupType)
        {
            var suffix = backupType switch
            {
                "items" => "items-images",
                "categories" => "categories-images",
                _ => "images"
            };

            return $"{suffix}-backup-{DateTime.Now:yyyyMMddHHmmss}.zip";
        }
    }
}
