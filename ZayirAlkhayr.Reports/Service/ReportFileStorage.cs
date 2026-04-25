using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using ZayirAlkhayr.Reports.Configuration;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Service
{
    public class ReportFileStorage : IReportFileStorage
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ReportOptions _options;

        public ReportFileStorage(IWebHostEnvironment environment, IOptions<ReportOptions> options)
        {
            _environment = environment;
            _options = options?.Value ?? new ReportOptions();
        }

        public string CreateReportPath(ReportRequestContext context, string extension)
        {
            var folderPath = Path.Combine(_environment.WebRootPath, _options.OutputFolder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var prefix = new string((context.FileNamePrefix ?? "Report")
                .Select(character => char.IsLetterOrDigit(character) ? character : '_')
                .ToArray())
                .Trim('_');

            if (string.IsNullOrWhiteSpace(prefix))
            {
                prefix = "Report";
            }

            return Path.Combine(folderPath, $"{prefix}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{extension}");
        }
    }
}
