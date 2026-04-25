using iText.Html2pdf;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Font;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Reports.Configuration;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Service
{
    public class PDFHelper : IPDFHelper
    {
        private static readonly Regex AngularAttributesRegex = new Regex(@"(\s_nghost-[a-zA-Z0-9\-]+=""[^""]*"")|(\s_ngcontent-[a-zA-Z0-9\-]+=""[^""]*"")", RegexOptions.Compiled);
        private static readonly Regex HtmlCommentsRegex = new Regex(@"<!--(.*?)-->", RegexOptions.Compiled | RegexOptions.Singleline);

        private readonly IWebHostEnvironment _environment;
        private readonly IReportFileStorage _reportFileStorage;
        private readonly ILogger<PDFHelper> _logger;
        private readonly PdfFont _pdfFont;

        public PDFHelper(IWebHostEnvironment environment,IReportFileStorage reportFileStorage,ILogger<PDFHelper> logger)
        {
            _environment = environment;
            _reportFileStorage = reportFileStorage;
            _logger = logger;
            var fontPath = Path.Combine(_environment.WebRootPath, "Fonts", "Cairo-Regular.ttf");
            _pdfFont = PdfFontFactory.CreateFont(fontPath, iText.IO.Font.PdfEncodings.IDENTITY_H);
        }

        public Task<ReportFileResult> SaveHtmlResultAsync(string html, ReportRequestContext context, CancellationToken cancellationToken = default)
        {
            var filePath = _reportFileStorage.CreateReportPath(context, ".pdf");
            ConvertHtmlToPdf(ClearAngularAttributes(html), filePath, cancellationToken);

            return Task.FromResult(new ReportFileResult
            {
                FilePath = filePath,
                ContentType = "application/pdf",
                DownloadFileName = Path.GetFileName(filePath)
            });
        }

        private void ConvertHtmlToPdf(string html, string outputPath, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var wrappedHtml = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <style>
                        @page {{
                            size: A4;
                            margin: 12px;
                        }}

                        html, body {{
                            margin: 0;
                            padding: 0;
                            width: 100%;
                            height: 100%;
                        }}

                        * {{
                            box-sizing: border-box;
                        }}

                        body {{
                            -webkit-print-color-adjust: exact;
                            print-color-adjust: exact;
                            font-family: 'Cairo';
                        }}
                    </style>
                </head>
                <body>
                    {html}
                </body>
                </html>";

                var writerProperties = new WriterProperties().SetFullCompressionMode(true);
                using (FileStream pdfStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (PdfWriter writer = new PdfWriter(pdfStream, writerProperties))
                using (PdfDocument pdfDocument = new PdfDocument(writer))
                {
                    var fontProvider = new FontProvider();
                    fontProvider.AddFont(_pdfFont.GetFontProgram());
                    fontProvider.AddFont(Path.Combine(_environment.WebRootPath, "Fonts", "Cairo-Black.ttf"));

                    var converterProperties = new ConverterProperties();
                    converterProperties.SetCharset("UTF-8");
                    converterProperties.SetBaseUri(_environment.WebRootPath);
                    converterProperties.SetFontProvider(fontProvider);

                    Document document = HtmlConverter.ConvertToDocument(wrappedHtml, pdfDocument, converterProperties);
                    document.SetMargins(0, 0, 0, 0);
                    document.Close();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to export report PDF to '{OutputPath}'.", outputPath);
                throw;
            }
        }

        private static string ClearAngularAttributes(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return string.Empty;
            }

            var result = AngularAttributesRegex.Replace(html, string.Empty);
            result = HtmlCommentsRegex.Replace(result, string.Empty);
            return result.Replace("&#x27;", "'", StringComparison.Ordinal);
        }
    }
}
