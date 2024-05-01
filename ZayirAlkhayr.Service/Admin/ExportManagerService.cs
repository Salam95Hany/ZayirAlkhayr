using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PosSystem.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using ZayirAlkhayr.Interface.Admin;

namespace ZayirAlkhayr.Service.Admin
{
    public class ExportManagerService : IExportManagerService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ExportManagerService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string Export(ExportTemplateBase exportTemplateBase, DataTable data)
        {
            var localPath = GetLocalPath(exportTemplateBase.TemplateName, ".xlsx");
            Export(localPath, data, _hostingEnvironment, exportTemplateBase.SubstitutionDictionary());
            return GetDownloadUrl(Path.GetFileName(localPath));
        }

        public void Export(string fullPath, DataTable data, IWebHostEnvironment hostingEnvironment, Dictionary<string, string> substitutionValue = null)
        {
            int startrow = 5;
            try
            {
                var temp = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, @"Template\", "ZAStyle.xlsx"));
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(new FileInfo(fullPath), temp))
                {
                    var sheet = package.Workbook.Worksheets["Sheet1"];

                    WriteHeader(sheet, data.Columns.Cast<DataColumn>().Select(e => e.ColumnName).ToList());
                    for (var i = 0; i < data.Rows.Count; ++i)
                    {
                        WriteRow(sheet, data.Rows[i].ItemArray, startrow);
                        startrow++;
                    }

                    SetTemplateValues(ref sheet, substitutionValue);
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
            TimeCell.Value = "تاريخ التحميل : " + String.Format("{0:dddd d , MMMM, yyyy}", DateTime.Now.ToString("dddd d , MMMM, yyyy", new CultureInfo("ar-AE")));
            var ByCell = worksheet.Cells[3, 10];
            ByCell.Value = "المستخدم : " + (substitutionValue["UserName"]);
            var IsValidSheetName = substitutionValue.TryGetValue("SheetName", out string sheetName);
            worksheet.Name = IsValidSheetName && !string.IsNullOrEmpty(sheetName) ? sheetName : "Sheet1";
        }

        private void WriteHeader(ExcelWorksheet worksheet, IList<string> headers, int? startRow = null)
        {
            try
            {
                for (var i = 0; i < headers.Count; i++)
                {
                    var headerValue = headers[i];
                    var headerCell = worksheet.Cells[startRow ?? 4, i + 1];
                    headerCell.Value = headerValue;
                    headerCell.AutoFitColumns(20);
                }
                ExcelRange cells = worksheet.Cells[startRow ?? 4, 1, startRow ?? 4, headers.Count];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetDownloadUrl(string FileName)
        {
            string URL = Path.Combine(_hostingEnvironment.WebRootPath, "ExportFiles", FileName);
            return URL;
        }
        private string GetLocalPath(string fileTitle, string extension)
        {
            string WEBurl = Path.Combine(_hostingEnvironment.WebRootPath, @"ExportFiles\", $"{fileTitle}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}");
            return WEBurl;
        }
    }
}
