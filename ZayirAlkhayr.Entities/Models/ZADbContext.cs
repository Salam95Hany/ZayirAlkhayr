using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZayirAlkhayr.Entities.Models
{
    public partial class ZADbContext: IdentityDbContext<AdminUser>
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

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivitySliderImage> ActivitiesSliderImage { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventSliderImages> EventSliderImages { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<Photos> Photos { get; set; }
        public DbSet<PhotoDetails> PhotoDetails { get; set; }
        public DbSet<Footer> Footers { get; set; }
        public DbSet<FamilyStatus> FamilyStatus { get; set; }
        public DbSet<FamilyStatusTypes> FamilyStatusTypes { get; set; }
        public DbSet<FamilyCategories> FamilyCategories { get; set; }
        public DbSet<FamilyDetails> FamilyDetails { get; set; }
        public DbSet<FamilyExpenses> FamilyExpenses { get; set; }
        public DbSet<FamilyExtraDetails> FamilyExtraDetails { get; set; }
        public DbSet<FamilyIncome> FamilyIncome { get; set; }
        public DbSet<FamilyNeeds> FamilyNeeds { get; set; }
        public DbSet<FamilyNeedTypes> FamilyNeedTypes { get; set; }
        public DbSet<FamilyPatient> FamilyPatient { get; set; }
        public DbSet<WebSiteVisitors> WebSiteVisitors { get; set; }
        public DbSet<BeneFactors> BeneFactors { get; set; }
        public DbSet<BeneFactorTypes> BeneFactorTypes { get; set; }
        public DbSet<BeneFactorValues> BeneFactorValues { get; set; }
        public DbSet<BeneFactorDetails> BeneFactorDetails { get; set; }

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
