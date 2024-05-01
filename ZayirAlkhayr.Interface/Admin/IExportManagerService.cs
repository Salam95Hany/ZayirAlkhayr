using PosSystem.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Interface.Admin
{
    public interface IExportManagerService
    {
        string Export(ExportTemplateBase exportTemplateBase, DataTable data);
    }
}
