using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    public partial class ZADbContext:DbContext
    {
        private IConfiguration Configuration;

        public ZADbContext(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public ZADbContext(DbContextOptions<DbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string ConnString = this.Configuration.GetConnectionString("DBConnection");
                optionsBuilder.UseSqlServer(ConnString);
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }
    }
}
