using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using RazorLight;
using System;
using System.IO;
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
using ZayirAlkhayr.Reports.Interface;
using ZayirAlkhayr.Reports.Service;
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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
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

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISQLHelper, SQLHelper>();
            services.AddScoped<IManageFileService, ManageFileService>();
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
            services.AddScoped<IItemRecipeService, ItemRecipeService>();
            services.AddScoped<IBackupService, BackupService>();
            services.AddScoped<IFactoryResetService, FactoryResetService>();

            #region ReportsDI

            services.AddSingleton<IRazorLightEngine>(serviceProvider =>
            {
                var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                var templatePath = Path.Combine(env.WebRootPath, "TemplatesHTML");
                return new RazorLightEngineBuilder()
                    .UseFileSystemProject(templatePath)
                    .UseMemoryCachingProvider()
                    .Build();
            });
            services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo<IReportGenerator>()).AsImplementedInterfaces().WithTransientLifetime());
            QuestPDF.Settings.License = LicenseType.Community;
            services.AddScoped<IReportGeneratorFactory, ReportGeneratorFactory>();
            services.AddScoped<IExportManagerService, ExportManagerService>();
            services.AddSingleton<IPDFHelper, PDFHelper>();


            #endregion

            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "POSRestaurant v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
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
