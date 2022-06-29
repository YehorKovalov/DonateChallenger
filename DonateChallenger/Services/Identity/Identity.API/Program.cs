using System.Reflection;
using Identity.API;
using Identity.API.Data;
using Identity.API.Data.Identities;
using Identity.API.Services;
using Identity.API.Services.Abstractions;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var appDbConnection = builder.Configuration["AppDbConnection"];
var configurationDbConnection = builder.Configuration["ConfigurationDbConnection"];

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appDbConnection));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(o =>
    {
        o.Password.RequiredLength = 4;
        o.Password.RequireUppercase = false;
        o.Password.RequireLowercase = false;
        o.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IHttpContextService, HttpContextService>();
builder.Services.AddTransient<IAccountService<ApplicationUser>, AccountService>();
builder.Services.Configure<AppSettings>(builder.Configuration);

var reactClientUrl = builder.Configuration?["ReactClientUrl"] ?? throw new ArgumentNullException();
var challengeCatalogUrl = builder.Configuration?["ChallengeCatalogUrl"] ?? throw new ArgumentNullException();
var globalUrl = builder.Configuration?["GlobalUrl"] ?? throw new ArgumentNullException();
builder.Services.AddCors(
    options => options
        .AddPolicy(
            "CorsPolicy",
            builder => builder
                .SetIsOriginAllowed((host) => true)
                .WithOrigins(challengeCatalogUrl, reactClientUrl, globalUrl)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddAspNetIdentity<ApplicationUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(configurationDbConnection, sql =>
            {
                sql.MigrationsAssembly(migrationsAssembly);
                sql.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(2), errorNumbersToAdd: null);
            });
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(configurationDbConnection, sql =>
            {
                sql.MigrationsAssembly(migrationsAssembly);
                sql.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(2), errorNumbersToAdd: null);
            });
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
app.UseCors("CorcPolicy");

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.CreateDbIfNotExist(new AppDbInitializer());
app.CreateDbIfNotExist(new ConfigurationDbContextInitializer());
app.Run();