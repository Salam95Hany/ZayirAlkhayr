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

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivitySliderImage> ActivitiesSliderImage { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<Footer> Footers { get; set; }
        public DbSet<FamilyStatus> FamilyStatus { get; set; }
        public DbSet<FamilyDetails> FamilyDetails { get; set; }
        public DbSet<FamilyExpenses> FamilyExpenses { get; set; }
        public DbSet<FamilyExtraDetails> FamilyExtraDetails { get; set; }
        public DbSet<FamilyIncome> FamilyIncome { get; set; }
        public DbSet<FamilyNeeds> FamilyNeeds { get; set; }
        public DbSet<FamilyPatient> FamilyPatient { get; set; }

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
