using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Interface
{
    public interface IReportFileStorage
    {
        string CreateReportPath(ReportRequestContext context, string extension);
    }
}
