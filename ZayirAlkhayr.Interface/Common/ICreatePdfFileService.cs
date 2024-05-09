using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface.Common
{
    public interface ICreatePdfFileService
    {
        string CreatePdfFile(DataTable Data, List<PDFHeaderSelected> HeaderNames, string FileName, string PageName);
    }
}
