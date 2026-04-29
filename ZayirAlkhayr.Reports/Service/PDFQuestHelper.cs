using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Reports.Service
{
    public class PDFQuestHelper : IDocument, IPDFQuestHelper
    {
        private readonly IWebHostEnvironment _env;
        private PurchaseData _model;

        public PDFQuestHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        // ── Public entry point ────────────────────────────────────────────────

        public async Task<string> GeneratePdf(PurchaseData model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

            string folder = Path.Combine(_env.WebRootPath, "Reports");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string safeInvoiceNo = string.IsNullOrWhiteSpace(model.PurchaseNumber)
                ? Guid.NewGuid().ToString("N")
                : model.PurchaseNumber.Replace("/", "-").Replace("\\", "-");

            string fileName = $"{safeInvoiceNo}.pdf";
            string fullPath = Path.Combine(folder, fileName);
            await Task.Run(() => this.GeneratePdf(fullPath));

            return fullPath;
        }

        // ── IDocument implementation ──────────────────────────────────────────

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x
                    .FontFamily("Arial")
                    .FontSize(11)
                    .DirectionFromRightToLeft());

                page.Content().Element(ComposeContent);
                page.Footer().ContentFromRightToLeft().DefaultTextStyle(x => x.FontFamily("Arial").FontSize(11).DirectionFromRightToLeft()).Element(Footer);
            });
        }

        // ── Main wrapper ──────────────────────────────────────────────────────

        private void ComposeContent(IContainer root)
        {
            root.ContentFromRightToLeft().Border(1).BorderColor(Palette.BorderGold)
                .Background(Palette.BgCream)
                .Column(col =>
                {
                    col.Item().Height(10).Background(Palette.Orange);
                    col.Item().Element(Header);
                    col.Item().Element(InfoSection);
                    col.Item().Height(3).Background(Palette.Orange);
                    col.Item().Element(ItemsTable);
                    col.Item().Element(TotalsRow);
                    //col.Item().Element(Footer);
                });
        }

        // ── Header ────────────────────────────────────────────────────────────

        private void Header(IContainer c)
        {
            c.BorderBottom(2).BorderColor(Palette.Orange)
             .Background(Palette.BgWarm)
             .Padding(5)
             .Row(row =>
             {
                 row.RelativeItem()
                    .AlignRight()
                    .AlignMiddle()
                    .Text("مطعم صبح و مسا")
                    .FontSize(22).Bold().FontColor(Palette.DarkBrown);

                 if (!string.IsNullOrEmpty(_model.ImageSrc) && File.Exists(_model.ImageSrc))
                 {
                     row.ConstantItem(80)
                        .AlignLeft()
                        .AlignMiddle()
                        .Width(80).Height(80)
                        .Image(_model.ImageSrc, ImageScaling.FitArea);
                 }
                 else
                 {
                     row.ConstantItem(80);
                 }
             });
        }

        // ── Info / fields ─────────────────────────────────────────────────────

        private void InfoSection(IContainer c)
        {
            c.PaddingVertical(15).PaddingHorizontal(30)
             .Column(col =>
             {
                 InfoRow(col, "فاتورة إلى :", _model.SupplierName,true);
                 InfoRow(col, "رقم الهاتف:", _model.SupplierPhone);
                 InfoRow(col, "التاريخ:", _model.InsertDateAr);
                 InfoRow(col, "رقم الفاتورة:", _model.PurchaseNumber);
             });
        }

        private static void InfoRow(ColumnDescriptor col, string label, string value, bool isArabic = false)
        {
            col.Item().Row(row =>
            {
                row.ConstantItem(140)
                   .AlignRight()
                   .Text(label)
                   .Bold()
                   .FontColor(Palette.Orange);
                if (!isArabic)
                    row.RelativeItem()
                       .BorderBottom(1).BorderColor(Palette.BorderGold)
                       .Height(22)
                       .AlignRight()
                       .Text(text =>
                       {
                           text.DefaultTextStyle(x => x.DirectionFromLeftToRight());
                           text.Span(value ?? "");
                       });
                else
                    row.RelativeItem()
                    .BorderBottom(1).BorderColor(Palette.BorderGold)
                    .Height(22)
                    .AlignRight()
                    .Text(value ?? "");
            });

            col.Item().Height(4);
        }

        // ── Items table ───────────────────────────────────────────────────────

        private void ItemsTable(IContainer c)
        {
            c.PaddingVertical(15).PaddingHorizontal(30)
             .Table(table =>
             {
                 table.ColumnsDefinition(cols =>
                 {
                     cols.RelativeColumn();
                     cols.ConstantColumn(95);
                     cols.ConstantColumn(70);
                     cols.ConstantColumn(70);
                     cols.ConstantColumn(95);
                 });

                 table.Header(h =>
                 {
                     h.Cell().Element(HeaderCell).AlignRight().Text("اسم الصنف");
                     h.Cell().Element(HeaderCell).AlignCenter().Text("السعر");
                     h.Cell().Element(HeaderCell).AlignCenter().Text("الكمية");
                     h.Cell().Element(HeaderCell).AlignCenter().Text("الوحدة");
                     h.Cell().Element(HeaderCell).AlignCenter().Text("الإجمالي");
                 });

                 for (int i = 0; i < _model.Items.Count; i++)
                 {
                     var item = _model.Items[i];
                     var bg = i % 2 == 0 ? Palette.BgCream : Palette.RowAlt;

                     table.Cell().Element(c2 => DataCell(c2, bg)).AlignRight().Text(item.InventoryItemName);
                     table.Cell().Element(c2 => DataCell(c2, bg)).AlignCenter().Text(t =>
                     {
                         t.DefaultTextStyle(x => x.DirectionFromLeftToRight());
                         t.Span(_model.FormatNumber(item.CostPrice));
                     });
                     table.Cell().Element(c2 => DataCell(c2, bg)).AlignCenter().Text(t =>
                     {
                         t.DefaultTextStyle(x => x.DirectionFromLeftToRight());
                         t.Span(_model.FormatNumber(item.Quantity));
                     });
                     table.Cell().Element(c2 => DataCell(c2, bg)).AlignCenter().Text(item.UnitName);
                     table.Cell().Element(c2 => DataCell(c2, bg)).AlignCenter().Text(t =>
                     {
                         t.DefaultTextStyle(x => x.DirectionFromLeftToRight());
                         t.Span(_model.FormatNumber(item.Total));
                     });
                 }
             });
        }

        private static IContainer HeaderCell(IContainer cell)
        {
            return cell.Background(Palette.Orange)
                       .Border(1).BorderColor(Palette.Orange)
                       .Padding(5)
                       .DefaultTextStyle(x => x.Bold().FontColor(Palette.White));
        }

        private static IContainer DataCell(IContainer cell, string bg)
        {
            return cell.Background(bg)
                       .Border(1).BorderColor(Palette.BorderGold)
                       .Padding(5);
        }

        // ── Totals ────────────────────────────────────────────────────────────

        private void TotalsRow(IContainer c)
        {
            c.PaddingVertical(15).PaddingHorizontal(30)
             .Row(row =>
             {
                 row.RelativeItem(75).Row(r =>
                 {
                     r.AutoItem().AlignMiddle()
                      .Text("المجموع بالحروف: ").Bold().FontColor(Palette.Orange);

                     r.RelativeItem()
                      .BorderBottom(1).BorderColor(Palette.BorderGold)
                      .Height(22).Padding(2)
                      .Text(_model.TotalAmountAr ?? "");
                 });

                 row.ConstantItem(20);

                 row.RelativeItem(25).Row(r =>
                 {
                     r.AutoItem().AlignMiddle()
                      .Text("المجموع: ").Bold().FontColor(Palette.Orange);

                     r.RelativeItem()
                      .BorderBottom(1).BorderColor(Palette.BorderGold)
                      .Height(22).Padding(2)
                      .AlignRight()
                      .Text(t =>
                      {
                          t.DefaultTextStyle(x => x.DirectionFromLeftToRight());
                          t.Span(_model.FormatNumber(_model.TotalAmount));
                      });
                 });
             });
        }

        // ── Footer ────────────────────────────────────────────────────────────

        private void Footer(IContainer c)
        {
            c.Column(col =>
            {
                col.Item().Height(12).Background(Palette.DarkBrown);

                col.Item().Background(Palette.LightOrange)
                   .PaddingHorizontal(8).PaddingVertical(6)
                   .Row(row =>
                   {
                       row.RelativeItem(30).AlignRight()
                          .Text(t =>
                          {
                              t.DefaultTextStyle(x => x.DirectionFromLeftToRight().FontColor(Palette.White).FontSize(10));
                              t.Span("0998222283 - 0998222286");
                          });

                       row.ConstantItem(20);

                       row.RelativeItem(60).AlignLeft()
                          .Text("درعا جاسم شرق المركز الثقافي")
                          .FontColor(Palette.White).FontSize(10);
                   });

                col.Item().Height(12).Background(Palette.Orange);
            });
        }
    }

    internal static class Palette
    {
        public const string Orange = "#E07B00";
        public const string DarkBrown = "#7A3A00";
        public const string LightOrange = "#C45E00";
        public const string BorderGold = "#C8A060";
        public const string BgCream = "#FFFDF9";
        public const string BgWarm = "#FFF8EE";
        public const string RowAlt = "#FFF4E0";
        public const string White = "#FFFFFF";
    }
}
