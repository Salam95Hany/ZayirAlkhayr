using System.Threading;
using System.Threading.Tasks;
using ZayirAlkhayr.Reports.Model;

namespace ZayirAlkhayr.Reports.Interface
{
    public interface IPDFHelper
    {
        string SaveHTMLResult(string HTML);
    }
}
