using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PosSystem.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface.Common;

namespace ZayirAlkhayr.Service.Common
{
    public class ExportManagerService : IExportManagerService
    {
        private readonly IWebHostEnvironment _environment;
        public ExportManagerService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string Export(ExportTemplateBase exportTemplateBase, DataTable data)
        {
            var localPath = GetLocalPath(exportTemplateBase.TemplateName, ".xlsx");
            Export(localPath, data, _environment, exportTemplateBase);
            return GetDownloadUrl(Path.GetFileName(localPath));
        }

        public void Export(string fullPath, DataTable data, IWebHostEnvironment hostingEnvironment, ExportTemplateBase exportTemplateBase)
        {
            int startrow = 5;
            try
            {
                var columnsToRemove = data.Columns.Cast<DataColumn>().ToList();
                columnsToRemove.ForEach(col => data.Columns.Remove(col));
                var temp = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, @"Template\", "ZAStyle.xlsx"));
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(new FileInfo(fullPath), temp))
                {
                    var sheet = package.Workbook.Worksheets["Sheet1"];
                    var worksheet = package.Workbook.Worksheets.Add("RightToLeft");
                    exportTemplateBase.Header.TblHeaders = data.Columns.Cast<DataColumn>().Select(e => e.ColumnName).ToList();
                    WriteHeader(sheet, exportTemplateBase.Header);
                    for (var i = 0; i < data.Rows.Count; ++i)
                    {
                        WriteRow(sheet, data.Rows[i].ItemArray, startrow);
                        startrow++;
                    }
                    var substitutionValue = exportTemplateBase.SubstitutionDictionary();
                    SetTemplateValues(ref sheet, substitutionValue);
                    worksheet.View.RightToLeft = true;
                    package.Save();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private void WriteRow(ExcelWorksheet worksheet, IList<object> values, int rowId)
        {
            ExcelRange cells = worksheet.Cells[rowId, 1, rowId, values.Count];
            cells.Style.Font.Name = "Arial";
            cells.Style.Font.Size = 12;
            cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            try
            {
                for (var i = 0; i < values.Count; i++)
                {
                    worksheet.Cells[rowId, i + 1].Value = values[i];
                }
            }
            catch (Exception ex)
            { }
        }

        private void SetTemplateValues(ref ExcelWorksheet worksheet, Dictionary<string, string> substitutionValue)
        {
            var TimeCell = worksheet.Cells[2, 10];
            TimeCell.Value = "تاريخ التحميل : " + DateTime.Now.ToString("dddd d MMMM , yyyy", new CultureInfo("ar-AE"));
            var ByCell = worksheet.Cells[3, 10];
            ByCell.Value = "اسم المستخدم : " + substitutionValue["UserName"];
            var IsValidSheetName = substitutionValue.TryGetValue("SheetName", out string sheetName);
            worksheet.Name = IsValidSheetName && !string.IsNullOrEmpty(sheetName) ? sheetName : "Sheet1";
        }

        private void WriteHeader(ExcelWorksheet worksheet, ExportHeaders Headers)
        {
            try
            {
                for (var i = 0; i < Headers.TblHeaders.Count; i++)
                {
                    var headerName = Headers.TblHeaders[i];
                    var header = new { NameAr = string.Empty };// Headers.ListHeaders.FirstOrDefault(i => i.NameEn == headerName);
                    var headerValue = header.NameAr;
                    var headerCell = worksheet.Cells[4, i + 1];
                    headerCell.Value = headerValue;
                    headerCell.AutoFitColumns(20);
                }
                ExcelRange cells = worksheet.Cells[4, 1, 4, Headers.TblHeaders.Count];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetDownloadUrl(string FileName)
        {
            string URL = Path.Combine(_environment.WebRootPath, "ExportFiles", FileName);
            return URL;
        }
        private string GetLocalPath(string fileTitle, string extension)
        {
            string WEBurl = Path.Combine(_environment.WebRootPath, @"ExportFiles\", $"{fileTitle}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}");
            return WEBurl;
        }
    }
}
