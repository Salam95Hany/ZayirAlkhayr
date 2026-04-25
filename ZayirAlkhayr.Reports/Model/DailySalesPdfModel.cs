using System.Collections.Generic;

namespace ZayirAlkhayr.Reports.Model
{
    public class DailySalesPdfModel
    {
        public string Title { get; set; } = string.Empty;
        public string GeneratedAt { get; set; } = string.Empty;
        public string GeneratedBy { get; set; } = string.Empty;
        public IReadOnlyList<PdfKeyValueItem> SummaryItems { get; set; } = new List<PdfKeyValueItem>();
        public IReadOnlyList<PdfKeyValueItem> AppliedFilters { get; set; } = new List<PdfKeyValueItem>();
        public IReadOnlyList<string> DetailHeaders { get; set; } = new List<string>();
        public IReadOnlyList<IReadOnlyList<string>> DetailRows { get; set; } = new List<IReadOnlyList<string>>();
        public string PreviewNotice { get; set; } = string.Empty;
        public bool IsRightToLeft { get; set; }
    }

    public class PdfKeyValueItem
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
