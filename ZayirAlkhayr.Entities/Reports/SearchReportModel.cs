using System;
using System.Collections.Generic;
using System.Linq;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Entities.Reports
{
    public class SearchReportModel
    {
        public string ReportType { get; set; } = string.Empty;
        public string UserName { get; set; }
        public string OutputFormat { get; set; }
        public string Culture { get; set; }
        public string DateFormat { get; set; }
        public string FileNamePrefix { get; set; }
        public List<QueryString> QueryString { get; set; } = new();
        public List<FilterModel> FilterList { get; set; } = new();

        public string GetQueryValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || QueryString == null || QueryString.Count == 0)
            {
                return string.Empty;
            }

            return QueryString.FirstOrDefault(query =>
                string.Equals(query.Key, key, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
        }
    }

    public class QueryString
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
