using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Reports.Model
{
    public class PdfDataReports
    {
        public DataTable Data { get; set; }
        public string ImageSrc { get; set; }

        public string GetDataFieldValue(string key)
        {
            if (Data == null || Data.Rows.Count == 0)
                return string.Empty;

            if (!Data.Columns.Contains(key))
                return string.Empty;

            var value = Data.Rows[0][key]?.ToString();

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            if (DateTime.TryParse(value, out DateTime date))
                return date.ToString("yyyy-MM-dd");

            return value;
        }
    }
}
