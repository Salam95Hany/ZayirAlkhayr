using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Interface
{
    public interface IReportGeneratorFactory
    {
        IReportGenerator GetGenerator(ReportType type, ExportFormat format);
    }
}
