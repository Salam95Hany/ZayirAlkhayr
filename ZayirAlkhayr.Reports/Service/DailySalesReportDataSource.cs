using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Reports.Configuration;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Service
{
    public class DailySalesReportDataSource : IDailySalesReportDataSource
    {
        private const string DailySalesDetailsProcedure = "[Report].[SP_ReportDailySalesDetails]";
        private const string DailySalesSummaryProcedure = "[Report].SP_GetReportDailySalesSummary";
        private static readonly Regex ColumnNameRegex = new Regex("([a-z])([A-Z])", RegexOptions.Compiled);

        private readonly ISQLHelper _sqlHelper;
        private readonly ReportOptions _options;
        private readonly ILogger<DailySalesReportDataSource> _logger;
        private readonly ConcurrentDictionary<string, CacheEntry<DailySalesPdfModel>> _summaryCache = new();

        public DailySalesReportDataSource(
            ISQLHelper sqlHelper,
            IOptions<ReportOptions> options,
            ILogger<DailySalesReportDataSource> logger)
        {
            _sqlHelper = sqlHelper;
            _options = options?.Value ?? new ReportOptions();
            _logger = logger;
        }

        public async IAsyncEnumerable<ReportDataBatch> ReadDetailsBatchesAsync(
            SearchReportModel request,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var currentPage = 1;
            var returnedAnyBatch = false;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var batch = await _sqlHelper.ExecuteReaderAsync(
                    DailySalesDetailsProcedure,
                    async reader =>
                    {
                        var columns = Enumerable.Range(0, reader.FieldCount)
                            .Select(reader.GetName)
                            .ToArray();

                        var rows = new List<object[]>(_options.DataSourcePageSize);
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            var values = new object[reader.FieldCount];
                            reader.GetValues(values);
                            rows.Add(values);
                        }

                        return new ReportDataBatch(columns, rows);
                    },
                    BuildDetailsParameters(request, currentPage));

                if (!returnedAnyBatch || batch.Rows.Count > 0 || batch.Columns.Count > 0)
                {
                    returnedAnyBatch = true;
                    yield return batch;
                }

                if (batch.Rows.Count < _options.DataSourcePageSize)
                {
                    yield break;
                }

                currentPage++;
            }
        }

        public async Task<DailySalesPdfModel> BuildPdfModelAsync(SearchReportModel request,ReportRequestContext context,CancellationToken cancellationToken = default)
        {
            ClearExpiredCacheEntries();

            var cacheKey = BuildCacheKey(request, context);
            if (_summaryCache.TryGetValue(cacheKey, out var cached) && cached.ExpiresAtUtc > DateTimeOffset.UtcNow)
            {
                _logger.LogDebug("Daily sales PDF summary cache hit for key {CacheKey}.", cacheKey);
                return cached.Value;
            }

            var summaryTable = await _sqlHelper.ExecuteDataTableAsync(
                DailySalesSummaryProcedure,
                BuildSummaryParameters(request));

            var previewHeaders = Array.Empty<string>();
            var previewRows = new List<IReadOnlyList<string>>();

            await foreach (var batch in ReadDetailsBatchesAsync(request, cancellationToken))
            {
                if (batch.Columns.Count > 0 && previewHeaders.Length == 0)
                {
                    previewHeaders = batch.Columns.ToArray();
                }

                foreach (var row in batch.Rows)
                {
                    previewRows.Add(row.Select(value => FormatScalarValue(value, context)).ToArray());

                    if (previewRows.Count >= _options.PdfPreviewRowCount)
                    {
                        break;
                    }
                }

                if (previewRows.Count >= _options.PdfPreviewRowCount)
                {
                    break;
                }
            }

            var model = new DailySalesPdfModel
            {
                Title = context.ReportDisplayName,
                GeneratedAt = context.GeneratedAt.ToString(context.DateFormat, context.Culture),
                GeneratedBy = context.GeneratedBy,
                SummaryItems = BuildSummaryItems(summaryTable, context),
                AppliedFilters = BuildAppliedFilters(request),
                DetailHeaders = previewHeaders,
                DetailRows = previewRows,
                PreviewNotice = previewRows.Count >= _options.PdfPreviewRowCount
                    ? $"تم عرض أول {_options.PdfPreviewRowCount} صف فقط لتقليل زمن التوليد وحجم الملف."
                    : string.Empty,
                IsRightToLeft = context.IsRightToLeft
            };

            _summaryCache[cacheKey] = new CacheEntry<DailySalesPdfModel>(
                model,
                DateTimeOffset.UtcNow.AddMinutes(_options.SummaryCacheDurationMinutes));

            return model;
        }

        private SqlParameter[] BuildDetailsParameters(SearchReportModel request, int currentPage)
        {
            var fromDate = request.FilterList?.FirstOrDefault(filter => filter.CategoryName == "DateRange")?.From;
            var toDate = request.FilterList?.FirstOrDefault(filter => filter.CategoryName == "DateRange")?.To;
            var filterTable = CreateFilterDataTable(request.FilterList);

            return new[]
            {
                new SqlParameter("@CurrentPage", currentPage),
                new SqlParameter("@PageSize", _options.DataSourcePageSize),
                new SqlParameter("@FilterList", filterTable),
                new SqlParameter("@FromDate", string.IsNullOrWhiteSpace(fromDate) ? DBNull.Value : (object)fromDate!),
                new SqlParameter("@ToDate", string.IsNullOrWhiteSpace(toDate) ? DBNull.Value : (object)toDate!)
            };
        }

        private SqlParameter[] BuildSummaryParameters(SearchReportModel request)
        {
            var fromDate = request.FilterList?.FirstOrDefault(filter => filter.CategoryName == "DateRange")?.From;
            var toDate = request.FilterList?.FirstOrDefault(filter => filter.CategoryName == "DateRange")?.To;
            var filterTable = CreateFilterDataTable(request.FilterList);

            return new[]
            {
                new SqlParameter("@FilterList", filterTable),
                new SqlParameter("@FromDate", string.IsNullOrWhiteSpace(fromDate) ? DBNull.Value : (object)fromDate!),
                new SqlParameter("@ToDate", string.IsNullOrWhiteSpace(toDate) ? DBNull.Value : (object)toDate!)
            };
        }

        private static DataTable CreateFilterDataTable(IEnumerable<FilterModel>? filters)
        {
            var table = new DataTable();
            table.Columns.Add("CategoryName", typeof(string));
            table.Columns.Add("ItemId", typeof(string));

            if (filters == null)
            {
                return table;
            }

            foreach (var filter in filters.Where(filter => filter != null))
            {
                if (!string.IsNullOrWhiteSpace(filter.ItemId))
                {
                    table.Rows.Add(filter.CategoryName ?? string.Empty, filter.ItemId);
                }

                if (filter.FilterItems == null)
                {
                    continue;
                }

                foreach (var item in filter.FilterItems.Where(item => item != null && !string.IsNullOrWhiteSpace(item.ItemId)))
                {
                    table.Rows.Add(item.CategoryName ?? filter.CategoryName ?? string.Empty, item.ItemId);
                }
            }

            return table;
        }

        private List<PdfKeyValueItem> BuildSummaryItems(DataTable summaryTable, ReportRequestContext context)
        {
            var summaryItems = new List<PdfKeyValueItem>();
            if (summaryTable == null || summaryTable.Rows.Count == 0)
            {
                return summaryItems;
            }

            var firstRow = summaryTable.Rows[0];
            foreach (DataColumn column in summaryTable.Columns)
            {
                var value = FormatScalarValue(firstRow[column], context);
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                summaryItems.Add(new PdfKeyValueItem
                {
                    Label = NormalizeColumnName(column.ColumnName),
                    Value = value
                });
            }

            return summaryItems;
        }

        private List<PdfKeyValueItem> BuildAppliedFilters(SearchReportModel request)
        {
            var filterItems = new List<PdfKeyValueItem>();
            if (request.FilterList == null || request.FilterList.Count == 0)
            {
                return filterItems;
            }

            foreach (var filter in request.FilterList.Where(filter => filter != null))
            {
                if (!string.IsNullOrWhiteSpace(filter.From) || !string.IsNullOrWhiteSpace(filter.To))
                {
                    filterItems.Add(new PdfKeyValueItem
                    {
                        Label = string.IsNullOrWhiteSpace(filter.CategoryDisplayName)
                            ? NormalizeColumnName(filter.CategoryName)
                            : filter.CategoryDisplayName,
                        Value = $"{filter.From} - {filter.To}".Trim(' ', '-')
                    });
                    continue;
                }

                var values = new List<string>();
                if (!string.IsNullOrWhiteSpace(filter.ItemValue))
                {
                    values.Add(filter.ItemValue);
                }

                if (!string.IsNullOrWhiteSpace(filter.ItemKey) && values.Count == 0)
                {
                    values.Add(filter.ItemKey);
                }

                if (filter.FilterItems != null)
                {
                    values.AddRange(filter.FilterItems
                        .Where(item => item != null && (!string.IsNullOrWhiteSpace(item.ItemValue) || !string.IsNullOrWhiteSpace(item.ItemKey)))
                        .Select(item => !string.IsNullOrWhiteSpace(item.ItemValue) ? item.ItemValue! : item.ItemKey!));
                }

                var distinctValues = values
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (distinctValues.Count == 0)
                {
                    continue;
                }

                filterItems.Add(new PdfKeyValueItem
                {
                    Label = string.IsNullOrWhiteSpace(filter.CategoryDisplayName)
                        ? NormalizeColumnName(filter.CategoryName)
                        : filter.CategoryDisplayName,
                    Value = string.Join("، ", distinctValues)
                });
            }

            return filterItems;
        }

        private string FormatScalarValue(object? value, ReportRequestContext context)
        {
            if (value == null || value == DBNull.Value)
            {
                return string.Empty;
            }

            return value switch
            {
                DateTime dateTimeValue => dateTimeValue.ToString(context.DateFormat, context.Culture),
                DateTimeOffset dateTimeOffsetValue => dateTimeOffsetValue.ToString(context.DateFormat, context.Culture),
                IFormattable formattableValue => formattableValue.ToString(null, context.Culture),
                _ => Convert.ToString(value) ?? string.Empty
            };
        }

        private string NormalizeColumnName(string? columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                return string.Empty;
            }

            var normalized = columnName.Replace("_", " ", StringComparison.Ordinal);
            normalized = ColumnNameRegex.Replace(normalized, "$1 $2");
            return normalized.Trim();
        }

        private string BuildCacheKey(SearchReportModel request, ReportRequestContext context)
        {
            var filterKey = request.FilterList == null
                ? string.Empty
                : string.Join("|", request.FilterList.Select(filter =>
                    $"{filter?.CategoryName}:{filter?.ItemId}:{filter?.ItemValue}:{filter?.From}:{filter?.To}"));

            return $"{context.ReportType}:{context.Culture.Name}:{context.DateFormat}:{filterKey}";
        }

        private void ClearExpiredCacheEntries()
        {
            var now = DateTimeOffset.UtcNow;
            foreach (var expiredEntry in _summaryCache.Where(entry => entry.Value.ExpiresAtUtc <= now).ToList())
            {
                _summaryCache.TryRemove(expiredEntry.Key, out _);
            }
        }

        private sealed class CacheEntry<T>
        {
            public CacheEntry(T value, DateTimeOffset expiresAtUtc)
            {
                Value = value;
                ExpiresAtUtc = expiresAtUtc;
            }

            public T Value { get; }
            public DateTimeOffset ExpiresAtUtc { get; }
        }
    }
}
