using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Service.Common
{
    public class TempPhysicalFileResult : PhysicalFileResult
    {
        public TempPhysicalFileResult(string fileName, string contentType)
                 : base(fileName, contentType) { }


        public override async Task ExecuteResultAsync(ActionContext context)
        {
            try
            {
                await base.ExecuteResultAsync(context);
            }
            finally
            {
                File.Delete(FileName);
            }
        }
    }
}
