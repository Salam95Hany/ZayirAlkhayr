using System;
using System.Globalization;

namespace ZayirAlkhayr.Reports.Model
{
    public class ReportRequestContext
    {
        public ReportRequestContext(
            ReportType reportType,
            ExportFormat format,
            CultureInfo culture,
            string dateFormat,
            string reportDisplayName,
            string fileNamePrefix,
            string sheetName,
            string templateKey,
            string generatedBy,
            bool isRightToLeft,
            DateTimeOffset generatedAt)
        {
            ReportType = reportType;
            Format = format;
            Culture = culture;
            DateFormat = dateFormat;
            ReportDisplayName = reportDisplayName;
            FileNamePrefix = fileNamePrefix;
            SheetName = sheetName;
            TemplateKey = templateKey;
            GeneratedBy = generatedBy;
            IsRightToLeft = isRightToLeft;
            GeneratedAt = generatedAt;
        }

        public ReportType ReportType { get; }
        public ExportFormat Format { get; }
        public CultureInfo Culture { get; }
        public string DateFormat { get; }
        public string ReportDisplayName { get; }
        public string FileNamePrefix { get; }
        public string SheetName { get; }
        public string TemplateKey { get; }
        public string GeneratedBy { get; }
        public bool IsRightToLeft { get; }
        public DateTimeOffset GeneratedAt { get; }
    }
}
