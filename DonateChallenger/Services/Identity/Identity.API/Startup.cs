using System;
using System.Reflection;
using Identity.API.Data;
using Identity.API.Extensions;
using Identity.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.API
{
    public class Startup
    {

        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appDbConnection = _configuration["AppDbConnection"];
            var configurationDbConnection = _configuration["ConfigurationDbConnection"];
            var persistedGrantDbConnection = _configuration["PersistedGrantDbConnection"];

            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appDbConnection));
            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
                {
                    o.Password.RequiredLength = 4;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            var reactClientUrl = _configuration?["ReactClientUrl"] ?? throw new ArgumentNullException();
            var challengeCatalogUrl = _configuration?["ChallengeCatalogUrl"] ?? throw new ArgumentNullException();
            var globalUrl = _configuration?["GlobalUrl"] ?? throw new ArgumentNullException();
            services.AddCors(
                options => options
                    .AddPolicy(
                        "CorsPolicy",
                        builder => builder
                            .SetIsOriginAllowed(host => true)
                            .WithOrigins(challengeCatalogUrl, reactClientUrl, globalUrl)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()));

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(configurationDbConnection, sql =>
                    {
                        sql.MigrationsAssembly(migrationsAssembly);
                        sql.EnableRetryOnFailure(15, TimeSpan.FromSeconds(2), null);
                    });
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(persistedGrantDbConnection, sql =>
                    {
                        sql.MigrationsAssembly(migrationsAssembly);
                        sql.EnableRetryOnFailure(15, TimeSpan.FromSeconds(2), null);
                    });
                });

            services.AddControllers();
            services.AddAuthentication();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

            app.CreateDbIfNotExist(new AppDbInitializer());
            app.CreateDbIfNotExist(new ConfigurationDbContextInitializer());
            app.CreateDbIfNotExist(new PersistedDbContextInitializer());
            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}