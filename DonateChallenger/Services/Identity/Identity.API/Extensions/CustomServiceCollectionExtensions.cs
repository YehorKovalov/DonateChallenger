using System.Reflection;
using Identity.API.Consumers;
using Identity.API.Data;
using Identity.API.Services;
using Identity.API.Services.Abstractions;
using Infrastructure.MessageBus.Extensions;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using MassTransit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Extensions;

public static class CustomServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredIdentity<TDbContext, TAppUser>(this IServiceCollection services)
        where TDbContext : IdentityDbContext<TAppUser>
        where TAppUser : IdentityUser
    {
        services.AddIdentity<TAppUser, IdentityRole>(o =>
            {
                o.Password.RequiredLength = 4;
                o.Password.RequireUppercase = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<TDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddConfiguredCors(this IServiceCollection services, IConfiguration configuration)
    {
            var reactClientUrl = configuration?["ReactClientUrl"] ?? throw new ArgumentNullException();
            var challengeCatalogUrl = configuration?["ChallengeCatalogUrl"] ?? throw new ArgumentNullException();
            var challengeOrderUrl = configuration?["ChallengeOrderUrl"] ?? throw new ArgumentNullException();
            var paymentUrl = configuration?["PaymentUrl"] ?? throw new ArgumentNullException();
            var challengesTemporaryStorageUrl = configuration?["ChallengesTemporaryStorageUrl"] ?? throw new ArgumentNullException();
            var globalUrl = configuration?["GlobalUrl"] ?? throw new ArgumentNullException();
        services.AddCors(
            options => options
                .AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(host => true)
                        .WithOrigins(
                            challengesTemporaryStorageUrl,
                            challengeOrderUrl,
                            paymentUrl,
                            challengeCatalogUrl,
                            reactClientUrl,
                            globalUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()));
        return services;
    }

    public static IServiceCollection AddConfiguredIdentityServer<TAppUser>(this IServiceCollection services, IConfiguration configuration)
        where TAppUser : IdentityUser
    {
        var configurationDbConnection = configuration["ConfigurationDbConnection"]  ?? throw new ArgumentNullException("ConfigurationDbConnection");
        var persistedGrantDbConnection = configuration["PersistedGrantDbConnection"]  ?? throw new ArgumentNullException("PersistedGrantDbConnection");
        var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
        
        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<TAppUser>()
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

        return services;
    }

    public static IServiceCollection ConfigureMvc(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddControllersWithViews();
        services.AddRazorPages();
        return services;
    }

    public static IServiceCollection AddAppDependencies(this IServiceCollection services)
    {
        services.AddTransient<IStreamerService, StreamerService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IAccountManagerService, AccountManagerService>();
        services.AddScoped<IProfileService, IdentityProfileService>();
        services.AddScoped<IDbContextWrapper<AppDbContext>, DbContextWrapper<AppDbContext>>();

        return services;
    }

    public static IServiceCollection AddConfiguredMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<MessageFindUsernamesByIdsConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureRabbitMqConnectionProperties(configuration);
                cfg.ReceiveEndpoint(configuration["RabbitMQ:FindUsernamesByIdsQueue"], c =>
                {
                    c.ConfigureConsumer<MessageFindUsernamesByIdsConsumer>(context);
                });
            });
        });

        services.AddMassTransitHostedService(true);
        return services;
    }
}