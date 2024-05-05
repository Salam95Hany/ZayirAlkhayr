using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Interface
{
    public interface ICreatePdfFileService
    {
        string CreatePdfFile(DataTable Data, List<string> HeaderNames, string FileName, string PageName);
    }
}
