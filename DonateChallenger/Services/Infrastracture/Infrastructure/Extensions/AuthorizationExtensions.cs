using System.IdentityModel.Tokens.Jwt;
using Infrastructure.Identity;
using Infrastructure.Identity.Configurations;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ClientConfig>(configuration.GetSection("Client"));
        services.Configure<AuthorizationConfig>(configuration.GetSection("Authorization"));

        var authority = configuration["Authorization:Authority"] ?? throw new ArgumentNullException("Authority");
        var audience = configuration["Authorization:SiteAudience"] ?? throw new ArgumentNullException("SiteAudience");

        services.AddSingleton<IAuthorizationHandler, ScopeHandler>();
        services
            .AddAuthentication(AuthScheme.Bearer)
            .AddJwtBearer(AuthScheme.Bearer, options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                options.RequireHttpsMetadata = false;
            });
        services.AddAuthorization(options =>
        {
            options.AddRolePolicy(AuthPolicy.DonaterPolicy, AppRoles.Donater);
            options.AddRolePolicy(AuthPolicy.AdminOnlyPolicy, AppRoles.Admin);
            options.AddRolePolicy(AuthPolicy.ManagerMinimumPolicy, AppRoles.Manager, AppRoles.Admin);
            options.AddRolePolicy(AuthPolicy.StreamerPolicy, AppRoles.Streamer);
        });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        return services;
    }

    private static void AddRolePolicy(this AuthorizationOptions options, string policyName, params string[] role)
    {
        options.AddPolicy(policyName, policy =>
        {
            policy.AuthenticationSchemes.Add(AuthScheme.Bearer);
            policy.AddRequirements(new ScopeRequirement());
            policy.RequireClaim("role", role);
        });
    }
}