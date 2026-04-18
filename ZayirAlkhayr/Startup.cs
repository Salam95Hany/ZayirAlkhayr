using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using System.Text;
using ZayirAlkhayr.Entities.Auth;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface.Auth;
using ZayirAlkhayr.Interface.Common;
using ZayirAlkhayr.Interface.Customer;
using ZayirAlkhayr.Interface.Inventory;
using ZayirAlkhayr.Interface.POS;
using ZayirAlkhayr.Interface.Report;
using ZayirAlkhayr.Interface.Repositories;
using ZayirAlkhayr.Interface.Setting;
using ZayirAlkhayr.Service.Auth;
using ZayirAlkhayr.Service.Common;
using ZayirAlkhayr.Service.Customer;
using ZayirAlkhayr.Service.Inventory;
using ZayirAlkhayr.Service.POS;
using ZayirAlkhayr.Service.Report;
using ZayirAlkhayr.Service.Repositories;
using ZayirAlkhayr.Service.Setting;

namespace ZayirAlkhayr
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_POSRestaurant";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var URLLists = Configuration.GetSection("URLList").Get<string[]>();
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(URLLists)
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            services.Configure<AppSettings>(Configuration);
            services.AddSingleton<IAppSettings>(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);
            services.AddControllers();
            services.AddDbContext<POSDbContext>();
            QuestPDF.Settings.License = LicenseType.Community;
            services.AddIdentity<AdminUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<POSDbContext>()
                .AddDefaultTokenProviders();

            var key = Encoding.UTF8.GetBytes("1234567890123456");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = System.TimeSpan.Zero
                };
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "POSRestaurant", Version = "v1" });
            });
            //QuestPDF.Settings.License = LicenseType.Community;
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISQLHelper, SQLHelper>();
            services.AddScoped<IManageFileService, ManageFileService>();
            services.AddScoped<IExportManagerService, ExportManagerService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ISalesReportService, SalesReportService>();
            services.AddScoped<IItemReportService, ItemReportService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<IInventoryAdjustmentService, InventoryAdjustmentService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IBackupService, BackupService>();
            services.AddScoped<IFactoryResetService, FactoryResetService>();

            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "POSRestaurant v1"));
            }

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
