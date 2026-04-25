using System;
using System.Collections.Generic;
using System.Linq;
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Service
{
    public class ReportGeneratorFactory : IReportGeneratorFactory
    {
        private readonly Dictionary<(ReportType ReportType, ExportFormat Format), IReportGenerator> _generators;

        public ReportGeneratorFactory(IEnumerable<IReportGenerator> generators)
        {
            var duplicateKeys = generators
                .GroupBy(generator => new { generator.ReportType, generator.Format })
                .Where(group => group.Count() > 1)
                .Select(group => $"{group.Key.ReportType}-{group.Key.Format}")
                .ToList();

            if (duplicateKeys.Count > 0)
            {
                throw new InvalidOperationException("Duplicate report generators detected: " + string.Join(", ", duplicateKeys));
            }

            _generators = generators.ToDictionary(
                generator => (generator.ReportType, generator.Format),
                generator => generator);
        }

        public IReportGenerator GetGenerator(ReportType type, ExportFormat format)
        {
            if (!_generators.TryGetValue((type, format), out var generator))
            {
                throw new NotSupportedException($"Unsupported report type '{type}' with format '{format}'.");
            }

            return generator;
        }
    }
}
