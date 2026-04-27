using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Reports;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Interface
{
    public interface IExportManagerService
    {
        string Export(ExportTemplateBase exportTemplateBase, DataTable data);
    }
}
