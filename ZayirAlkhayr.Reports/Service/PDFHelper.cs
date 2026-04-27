using iText.Html2pdf;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Font;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Text.RegularExpressions;
using ZayirAlkhayr.Reports.Interface;

namespace ZayirAlkhayr.Reports.Service
{
    public class PDFHelper : IPDFHelper
    {
        private readonly IWebHostEnvironment _environment;
        private readonly PdfFont _pdfFont;

        public PDFHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
            var fontPath = System.IO.Path.Combine(_environment.WebRootPath, "Fonts", "Cairo-Regular.ttf");
            _pdfFont = PdfFontFactory.CreateFont(fontPath, iText.IO.Font.PdfEncodings.IDENTITY_H);
        }

        public string SaveHTMLResult(string HTMLContent)
        {
            try
            {
                //HTMLContent = ClearAngularAttrFromHTML(HTMLContent);
                var FolderPath = System.IO.Path.Combine(_environment.WebRootPath, "Reports");

                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);

                var FilePath = System.IO.Path.Combine(FolderPath, "Report_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".pdf");
                ConvertHtmlToPdf(HTMLContent, FilePath);

                return FilePath;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ConvertHtmlToPdf(string html, string outputPath)
        {
            try
            {
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
                throw;
            }
        }


        public string ClearAngularAttrFromHTML(string HTML)
        {
            try
            {
                if (string.IsNullOrEmpty(HTML))
                    return HTML;

                HTML = Regex.Replace(HTML, "( _nghost-ng-cli-universal-c| _ngcontent-ng-cli-universal-c)[1-9]*=\"\"", "");
                HTML = Regex.Replace(HTML, "<!--([a-z]+)(?![^>]*\\/>)[^>]*-->", "");
                HTML = Regex.Replace(HTML, @"\s_ngcontent-[a-zA-Z0-9\-]+?=""[^""]*""", "");
                HTML = HTML.Replace("&#x27;", "");

                return HTML;
            }
            catch (Exception)
            {
                return HTML;
            }
        }
    }
}
