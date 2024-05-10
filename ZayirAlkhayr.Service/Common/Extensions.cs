using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Service.Common
{
    public static class Extensions
    {
        public static List<DataTable> ToDataTableBatches(this DataTable dt,int BatchNumber)
        {
            var batches = dt.AsEnumerable().Select((x, i) => new { Index = i, Value = x })
                 .GroupBy(x => x.Index / BatchNumber)
                 .Select(x => x.Select(v => v.Value).ToList().CopyToDataTable())
                 .ToList();

            return batches;
        }

        public static DataTable RemoveColumns(this DataTable dt, List<string> Headers)
        {
            var toRemove = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).Except(Headers).ToList();
            toRemove.ForEach(col => dt.Columns.Remove(col));

            return dt;
        }
    }
}
