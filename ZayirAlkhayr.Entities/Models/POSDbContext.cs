using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Auth;

namespace ZayirAlkhayr.Entities.Models
{
    public partial class POSDbContext: IdentityDbContext<AdminUser>
    {
        private IConfiguration Configuration;

        public POSDbContext(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public POSDbContext(DbContextOptions<DbContext> options)
           : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<ItemRecipe> ItemRecipes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<InventoryAdjustment> InventoryAdjustments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string ConnString = this.Configuration.GetConnectionString("DBConnection");
                optionsBuilder.UseSqlServer(ConnString);
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>().HasOne(i => i.Category).WithMany(c => c.Items).HasForeignKey(i => i.CategoryId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>().HasOne(i => i.Customers).WithMany(c => c.Orders).HasForeignKey(i => i.CustomerId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ItemRecipe>().HasOne(i => i.Item).WithMany(i => i.ItemRecipes).HasForeignKey(i => i.ItemId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ItemRecipe>().HasOne(i => i.InventoryItem).WithMany(i => i.ItemRecipes).HasForeignKey(i => i.InventoryItemId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ItemRecipe>().HasIndex(i => new { i.ItemId, i.InventoryItemId }).IsUnique();
        }
    }
}
