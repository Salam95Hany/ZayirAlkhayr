using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Reports.Configuration;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Service
{
    public class ExportManagerService : IExportManagerService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IReportFileStorage _reportFileStorage;
        private readonly ReportOptions _options;
        private readonly ILogger<ExportManagerService> _logger;

        public ExportManagerService(
            IWebHostEnvironment environment,
            IReportFileStorage reportFileStorage,
            IOptions<ReportOptions> options,
            ILogger<ExportManagerService> logger)
        {
            _environment = environment;
            _reportFileStorage = reportFileStorage;
            _options = options?.Value ?? new ReportOptions();
            _logger = logger;
        }

        public async Task<ReportFileResult> ExportAsync(ReportRequestContext context,IAsyncEnumerable<ReportDataBatch> dataBatches,CancellationToken cancellationToken = default)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (dataBatches == null)
            {
                throw new ArgumentNullException(nameof(dataBatches));
            }

            return context.Format switch
            {
                ExportFormat.Excel => await ExportExcelAsync(context, dataBatches, cancellationToken),
                ExportFormat.Csv => await ExportCsvAsync(context, dataBatches, cancellationToken),
                ExportFormat.Json => await ExportJsonAsync(context, dataBatches, cancellationToken),
                _ => throw new NotSupportedException($"Tabular export format '{context.Format}' is not supported.")
            };
        }

        private async Task<ReportFileResult> ExportExcelAsync(ReportRequestContext context,IAsyncEnumerable<ReportDataBatch> dataBatches,CancellationToken cancellationToken)
        {
            var filePath = _reportFileStorage.CreateReportPath(context, ".xlsx");
            var templatePath = Path.Combine(_environment.WebRootPath, _options.ExcelTemplateFolder, _options.ExcelTemplateFileName);
            _logger.LogInformation("Exporting report {ReportType} as Excel to {Path}.", context.ReportType, filePath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = CreateExcelPackage(filePath, templatePath))
            {
                var worksheetStates = new List<WorksheetState>();
                WorksheetState currentWorksheet = null;
                var sheetIndex = 0;

                await foreach (var batch in dataBatches.WithCancellation(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (batch.Columns.Count == 0)
                    {
                        continue;
                    }

                    if (currentWorksheet == null)
                    {
                        currentWorksheet = CreateWorksheetState(package, context, batch.Columns, ++sheetIndex);
                        worksheetStates.Add(currentWorksheet);
                    }

                    WriteExcelBatch(package, context, batch, worksheetStates, ref currentWorksheet, ref sheetIndex);
                }

                if (currentWorksheet == null)
                {
                    currentWorksheet = CreateWorksheetState(package, context, Array.Empty<string>(), ++sheetIndex);
                    worksheetStates.Add(currentWorksheet);
                }

                foreach (var worksheetState in worksheetStates)
                {
                    FinalizeWorksheet(worksheetState);
                }

                package.Save();
            }

            return CreateResult(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        private async Task<ReportFileResult> ExportCsvAsync(
            ReportRequestContext context,
            IAsyncEnumerable<ReportDataBatch> dataBatches,
            CancellationToken cancellationToken)
        {
            var filePath = _reportFileStorage.CreateReportPath(context, ".csv");
            _logger.LogInformation("Exporting report {ReportType} as CSV to {Path}.", context.ReportType, filePath);
            var lineBuilder = new StringBuilder(4096);
            var headerWritten = false;
            var rowCount = 0;

            await using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 131072, true))
            await using (var writer = new StreamWriter(stream, new UTF8Encoding(false), 131072))
            {
                await foreach (var batch in dataBatches.WithCancellation(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (!headerWritten && batch.Columns.Count > 0)
                    {
                        await WriteCsvRowAsync(writer, lineBuilder, batch.Columns.Cast<object>(), context, cancellationToken);
                        headerWritten = true;
                    }

                    foreach (var row in batch.Rows)
                    {
                        await WriteCsvRowAsync(writer, lineBuilder, row, context, cancellationToken);
                        rowCount++;

                        if (rowCount % _options.CsvFlushInterval == 0)
                        {
                            await writer.FlushAsync();
                        }
                    }
                }
            }

            return CreateResult(filePath, "text/csv");
        }

        private async Task<ReportFileResult> ExportJsonAsync(
            ReportRequestContext context,
            IAsyncEnumerable<ReportDataBatch> dataBatches,
            CancellationToken cancellationToken)
        {
            var filePath = _reportFileStorage.CreateReportPath(context, ".json");
            _logger.LogInformation("Exporting report {ReportType} as JSON to {Path}.", context.ReportType, filePath);
            var rowCount = 0;

            await using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 131072, true))
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
            {
                Indented = false,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }))
            {
                writer.WriteStartArray();

                await foreach (var batch in dataBatches.WithCancellation(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    foreach (var row in batch.Rows)
                    {
                        writer.WriteStartObject();
                        for (var columnIndex = 0; columnIndex < batch.Columns.Count; columnIndex++)
                        {
                            writer.WritePropertyName(batch.Columns[columnIndex]);
                            WriteJsonValue(writer, row[columnIndex]);
                        }

                        writer.WriteEndObject();
                        rowCount++;

                        if (rowCount % _options.JsonFlushInterval == 0)
                        {
                            await writer.FlushAsync(cancellationToken);
                        }
                    }
                }

                writer.WriteEndArray();
                await writer.FlushAsync(cancellationToken);
            }

            return CreateResult(filePath, "application/json");
        }

        private ExcelPackage CreateExcelPackage(string outputPath, string templatePath)
        {
            if (File.Exists(templatePath))
            {
                return new ExcelPackage(new FileInfo(outputPath), new FileInfo(templatePath));
            }

            return new ExcelPackage();
        }

        private WorksheetState CreateWorksheetState(ExcelPackage package,ReportRequestContext context,IReadOnlyList<string> columns,int sheetIndex)
        {
            var worksheet = ResolveWorksheet(package, context, sheetIndex);
            ResetWorksheet(worksheet);

            worksheet.View.RightToLeft = context.IsRightToLeft;
            worksheet.View.FreezePanes(_options.ExcelDataStartRowIndex, 1);
            worksheet.Cells.Style.Font.Name = context.IsRightToLeft ? "Cairo" : "Arial";
            worksheet.Cells.Style.Font.Size = 11;

            WriteSheetMetadata(worksheet, context, Math.Max(columns.Count, 1));
            if (columns.Count > 0)
            {
                WriteHeader(worksheet, columns);
            }

            return new WorksheetState
            {
                Worksheet = worksheet,
                ColumnCount = columns.Count,
                NextRow = _options.ExcelDataStartRowIndex
            };
        }

        private ExcelWorksheet ResolveWorksheet(ExcelPackage package, ReportRequestContext context, int sheetIndex)
        {
            if (sheetIndex == 1 && package.Workbook.Worksheets.Count > 0)
            {
                var worksheet = package.Workbook.Worksheets.First();
                worksheet.Name = BuildSheetName(context.SheetName, sheetIndex);
                return worksheet;
            }

            return package.Workbook.Worksheets.Add(BuildSheetName(context.SheetName, sheetIndex));
        }

        private void ResetWorksheet(ExcelWorksheet worksheet)
        {
            if (worksheet.Dimension != null)
            {
                worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].Clear();
            }
        }

        private void WriteExcelBatch(ExcelPackage package,ReportRequestContext context,ReportDataBatch batch,IList<WorksheetState> worksheetStates,ref WorksheetState currentWorksheet,ref int sheetIndex)
        {
            var currentIndex = 0;
            while (currentIndex < batch.Rows.Count)
            {
                var remainingRows = _options.MaxExcelRowsPerWorksheet - currentWorksheet!.NextRow + 1;
                if (remainingRows <= 0)
                {
                    currentWorksheet = CreateWorksheetState(package, context, batch.Columns, ++sheetIndex);
                    worksheetStates.Add(currentWorksheet);
                    remainingRows = _options.MaxExcelRowsPerWorksheet - currentWorksheet.NextRow + 1;
                }

                var rowsToWrite = Math.Min(remainingRows, batch.Rows.Count - currentIndex);
                var currentSlice = batch.Rows.Skip(currentIndex).Take(rowsToWrite).ToList();

                currentWorksheet.Worksheet.Cells[
                    currentWorksheet.NextRow,
                    1,
                    currentWorksheet.NextRow + rowsToWrite - 1,
                    batch.Columns.Count].LoadFromArrays(currentSlice);

                currentWorksheet.ColumnCount = batch.Columns.Count;
                currentWorksheet.NextRow += rowsToWrite;
                currentIndex += rowsToWrite;
            }
        }

        private void WriteSheetMetadata(ExcelWorksheet worksheet, ReportRequestContext context, int columnCount)
        {
            worksheet.Cells[1, 1, 1, columnCount].Merge = true;
            worksheet.Cells[1, 1].Value = context.ReportDisplayName;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            worksheet.Cells[2, 1, 2, columnCount].Merge = true;
            worksheet.Cells[2, 1].Value = $"Generated at: {context.GeneratedAt.ToString(context.DateFormat, context.Culture)}";
            worksheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            worksheet.Cells[3, 1, 3, columnCount].Merge = true;
            worksheet.Cells[3, 1].Value = $"Generated by: {context.GeneratedBy}";
            worksheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private void WriteHeader(ExcelWorksheet worksheet, IReadOnlyList<string> headers)
        {
            for (var index = 0; index < headers.Count; index++)
            {
                worksheet.Cells[_options.ExcelHeaderRowIndex, index + 1].Value = headers[index];
            }

            var headerCells = worksheet.Cells[_options.ExcelHeaderRowIndex, 1, _options.ExcelHeaderRowIndex, headers.Count];
            headerCells.Style.Font.Bold = true;
            headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(233, 237, 245));
            headerCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            headerCells.AutoFilter = true;
        }

        private void FinalizeWorksheet(WorksheetState worksheetState)
        {
            if (worksheetState.ColumnCount == 0)
            {
                return;
            }

            var previewLastRow = Math.Min(
                Math.Max(worksheetState.NextRow - 1, _options.ExcelHeaderRowIndex),
                _options.ExcelHeaderRowIndex + 50);

            worksheetState.Worksheet.Cells[_options.ExcelHeaderRowIndex, 1, previewLastRow, worksheetState.ColumnCount].AutoFitColumns();
        }

        private async Task WriteCsvRowAsync(
            StreamWriter writer,
            StringBuilder lineBuilder,
            IEnumerable<object> values,
            ReportRequestContext context,
            CancellationToken cancellationToken)
        {
            lineBuilder.Clear();
            var firstValue = true;

            foreach (var value in values)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!firstValue)
                {
                    lineBuilder.Append(',');
                }

                var formattedValue = FormatScalarValue(value, context);
                var escapedValue = formattedValue.Replace("\"", "\"\"", StringComparison.Ordinal);
                var mustQuote = escapedValue.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0;

                if (mustQuote)
                {
                    lineBuilder.Append('"').Append(escapedValue).Append('"');
                }
                else
                {
                    lineBuilder.Append(escapedValue);
                }

                firstValue = false;
            }

            await writer.WriteLineAsync(lineBuilder.ToString());
        }

        private string FormatScalarValue(object? value, ReportRequestContext context)
        {
            if (value == null || value == DBNull.Value)
            {
                return string.Empty;
            }

            return value switch
            {
                DateTime dateTimeValue => dateTimeValue.ToString(context.DateFormat, context.Culture),
                DateTimeOffset dateTimeOffsetValue => dateTimeOffsetValue.ToString(context.DateFormat, context.Culture),
                IFormattable formattableValue => formattableValue.ToString(null, context.Culture),
                _ => Convert.ToString(value) ?? string.Empty
            };
        }

        private static void WriteJsonValue(Utf8JsonWriter writer, object? value)
        {
            if (value == null || value == DBNull.Value)
            {
                writer.WriteNullValue();
                return;
            }

            switch (value)
            {
                case int intValue:
                    writer.WriteNumberValue(intValue);
                    break;
                case long longValue:
                    writer.WriteNumberValue(longValue);
                    break;
                case short shortValue:
                    writer.WriteNumberValue(shortValue);
                    break;
                case decimal decimalValue:
                    writer.WriteNumberValue(decimalValue);
                    break;
                case double doubleValue:
                    writer.WriteNumberValue(doubleValue);
                    break;
                case float floatValue:
                    writer.WriteNumberValue(floatValue);
                    break;
                case bool boolValue:
                    writer.WriteBooleanValue(boolValue);
                    break;
                case DateTime dateTimeValue:
                    writer.WriteStringValue(dateTimeValue);
                    break;
                case DateTimeOffset dateTimeOffsetValue:
                    writer.WriteStringValue(dateTimeOffsetValue);
                    break;
                case Guid guidValue:
                    writer.WriteStringValue(guidValue);
                    break;
                case byte[] bytes:
                    writer.WriteBase64StringValue(bytes);
                    break;
                default:
                    writer.WriteStringValue(Convert.ToString(value));
                    break;
            }
        }

        private string BuildSheetName(string sheetName, int sheetIndex)
        {
            var baseName = string.IsNullOrWhiteSpace(sheetName) ? "Sheet" : sheetName;
            var suffix = sheetIndex > 1 ? $"_{sheetIndex}" : string.Empty;
            var invalidCharacters = new[] { ':', '\\', '/', '?', '*', '[', ']' };
            var sanitized = new string(baseName.Where(character => !invalidCharacters.Contains(character)).ToArray());
            sanitized = string.IsNullOrWhiteSpace(sanitized) ? "Sheet" : sanitized;

            var finalName = sanitized + suffix;
            return finalName.Length > 31 ? finalName.Substring(0, 31) : finalName;
        }

        private ReportFileResult CreateResult(string filePath, string contentType)
        {
            return new ReportFileResult
            {
                FilePath = filePath,
                ContentType = contentType,
                DownloadFileName = Path.GetFileName(filePath)
            };
        }

        private sealed class WorksheetState
        {
            public ExcelWorksheet Worksheet { get; set; }
            public int ColumnCount { get; set; }
            public int NextRow { get; set; }
        }
    }
}
