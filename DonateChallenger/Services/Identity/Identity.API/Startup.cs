using Identity.API.Data;
using Identity.API.Data.Entities;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            services
                .AddDbContextFactory<AppDbContext>(options => options.UseSqlServer(appDbConnection))
                .AddConfiguredIdentity<AppDbContext, ApplicationUser>()
                .AddCustomAuthorization(_configuration)
                .AddConfiguredCors(_configuration)
                .AddAppDependencies()
                .AddConfiguredIdentityServer<ApplicationUser>(_configuration)
                .ConfigureMvc()
                .AddConfiguredMessageBus(_configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseIdentityServer();
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });
            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseAuthorization();

            app.EnsureInitializeAppDbContexts();
            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}