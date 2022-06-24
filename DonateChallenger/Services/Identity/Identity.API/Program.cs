using System.Reflection;
using Identity.API;
using Identity.API.Data;
using Identity.API.Data.Identities;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var appDbConnection = builder.Configuration.GetConnectionString("AppDbConnection");
var configurationDbConnection = builder.Configuration.GetConnectionString("ConfigurationDbConnection");

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appDbConnection));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddAspNetIdentity<ApplicationUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(
            configurationDbConnection,
            sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(
            configurationDbConnection,
            sql => sql.MigrationsAssembly(migrationsAssembly));
    });

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseIdentityServer();

app.UseRouting();
app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.CreateDbIfNotExist(new AppDbInitializer());
app.CreateDbIfNotExist(new ConfigurationDbContextInitializer());
app.Run();