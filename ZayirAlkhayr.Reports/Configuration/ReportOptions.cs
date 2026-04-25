namespace ZayirAlkhayr.Reports.Configuration
{
    public class ReportOptions
    {
        public string OutputFolder { get; set; } = "Reports";
        public string HtmlTemplateFolder { get; set; } = "TemplatesHTML";
        public string ExcelTemplateFolder { get; set; } = "Template";
        public string ExcelTemplateFileName { get; set; } = "POS_Temp.xlsx";
        public string DefaultCulture { get; set; } = "ar-EG";
        public string DefaultDateFormat { get; set; } = "dd/MM/yyyy HH:mm";
        public int ExcelHeaderRowIndex { get; set; } = 4;
        public int ExcelDataStartRowIndex { get; set; } = 5;
        public int ExcelBatchSize { get; set; } = 2000;
        public int DataSourcePageSize { get; set; } = 5000;
        public int CsvFlushInterval { get; set; } = 250;
        public int JsonFlushInterval { get; set; } = 250;
        public int PdfPreviewRowCount { get; set; } = 50;
        public int MaxExcelRowsPerWorksheet { get; set; } = 1048576;
        public int SummaryCacheDurationMinutes { get; set; } = 5;
    }
}
