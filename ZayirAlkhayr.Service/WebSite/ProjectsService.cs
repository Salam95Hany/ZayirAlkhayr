using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.WebSite;

namespace ZayirAlkhayr.Service.WebSite
{
    public class ProjectsService: IProjectsService
    {
        private readonly ZADbContext _Context;
        private readonly IManageFileService _manageFileService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ISQLHelper _sQLHelper;
        private string ApiLocalUrl;
        public ProjectsService(ZADbContext Context, IManageFileService manageFileService, IConfiguration configuration, IWebHostEnvironment environment, ISQLHelper sQLHelper)
        {
            _Context = Context;
            _manageFileService = manageFileService;
            _configuration = configuration;
            _environment = environment;
            _sQLHelper = sQLHelper;
            ApiLocalUrl = _configuration["ApiUrlLocal"];
        }
    }
}
