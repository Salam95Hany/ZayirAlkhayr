using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Interface.Common;

namespace ZayirAlkhayr.Service.Common
{
    public class CreatePdfFileService : ICreatePdfFileService
    {
        private readonly IWebHostEnvironment _environment;
        public CreatePdfFileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public string CreatePdfFile(DataTable Data, List<PDFHeaderSelected> HeaderNames, string FileName, string PageName)
        {
            var FullPath = Path.Combine(_environment.WebRootPath, "ExportFiles", FileName + ".pdf");
            var firstCol = Math.Round((decimal)(Data.Columns.Count / 2));
            var secondCol = Data.Columns.Count - firstCol;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Millimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));
                    page.ContentFromRightToLeft();

                    // Page Header
                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                         {
                             row.AutoItem().AlignRight().Text(txt =>
                             {
                                 txt.Line("مؤسسة زائر الخير").FontSize(15);
                                 txt.Line("المشهرة برقم 4242 لسنة 2021");
                                 txt.Line("تحت رعاية وزارة التضامن الاجتماعي");
                             });
                             row.AutoItem().PaddingHorizontal(180);
                             row.AutoItem().Height(85).Width(85).Image(Path.Combine(_environment.WebRootPath, "Template", "ZayirAlkhayrLogo2.jpeg"));
                         });

                        col.Item().AlignCenter().PaddingBottom(5).Text("كشف تسليم مساعدات مالية-محافظة الاسكندرية-      /      /      ").FontSize(15);
                        col.Item().LineHorizontal(2);
                        col.Item().AlignCenter().PaddingTop(5).PaddingBottom(5).Text(PageName).FontSize(15);

                    });

                    // Page Content
                    page.Content().Column(col =>
                    {
                        col.Item().MaxHeight(600).Table(tbl =>
                        {
                            IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                            {
                                return container
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Lighten1)
                                    .Background(backgroundColor)
                                    .PaddingVertical(5)
                                    .PaddingHorizontal(5);
                            }

                            tbl.ColumnsDefinition(columns =>
                            {
                                for (int i = 0; i < HeaderNames.Count; i++)
                                    columns.RelativeColumn();
                            });

                            tbl.Header(header =>
                            {
                                foreach (var name in HeaderNames)
                                {
                                    header.Cell().Element(CellStyle).Text(name.NameAr);
                                }
                                IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                            });
                            for (int i = 0; i < Data.Rows.Count; i++)
                            {
                                foreach (var column in HeaderNames)
                                {
                                    tbl.Cell().Element(CellStyle).Text(Data.Rows[i][column.NameEn].ToString());
                                }
                            }

                            IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.White).ShowOnce();
                            tbl.Footer(tblf =>
                            {
                                tblf.Cell().ColumnSpan(uint.Parse(firstCol.ToString())).Border(1).BorderColor(Colors.Black).PaddingVertical(5).PaddingHorizontal(5).AlignCenter().Text("إجمالي القيمة");
                                tblf.Cell().ColumnSpan(uint.Parse(secondCol.ToString())).Border(1).BorderColor(Colors.Black).PaddingVertical(5).PaddingHorizontal(5).Text(txt2 =>
                                {
                                    txt2.TotalPages();
                                });
                            });
                        });
                    });

                    // Page Footer
                    page.Footer().Column(col =>
                    {
                        col.Item().PaddingBottom(60).Row(row =>
                        {
                            row.AutoItem().AlignRight().Text("مسؤول التوزيع").FontSize(15);
                            row.AutoItem().PaddingHorizontal(190);
                            row.AutoItem().Text("رئيس مجلس الأمناء").FontSize(15);
                        });

                        col.Item().AlignCenter()
                        .Text(x =>
                        {
                            x.Span("الصفحة ");
                            x.CurrentPageNumber();
                        });
                    });

                });
            }).GeneratePdf(FullPath);

            return FullPath;
        }
    }
}
