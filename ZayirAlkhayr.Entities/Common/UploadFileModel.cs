using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Common
{
    public class UploadFileModel
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
    }
}
