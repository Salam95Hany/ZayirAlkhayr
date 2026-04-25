using System.Collections.Generic;

namespace ZayirAlkhayr.Reports.Model
{
    public class ReportDataBatch
    {
        public ReportDataBatch(IReadOnlyList<string> columns, IReadOnlyList<object[]> rows)
        {
            Columns = columns;
            Rows = rows;
        }

        public IReadOnlyList<string> Columns { get; }
        public IReadOnlyList<object[]> Rows { get; }
    }
}
