using System.IdentityModel.Tokens.Jwt;
using Infrastructure.Identity;
using Infrastructure.Identity.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ClientConfig>(configuration.GetSection("Client"));
        services.Configure<AuthorizationConfig>(configuration.GetSection("Authorization"));

        var authority = configuration["Authorization:Authority"] ?? throw new ArgumentNullException("Authority");
        var siteAudience = configuration["Authorization:SiteAudience"] ?? throw new ArgumentNullException("SiteAudience");

        services.AddSingleton<IAuthorizationHandler, ScopeHandler>();
        services
            .AddAuthentication()
            .AddJwtBearer(AuthScheme.Internal, options =>
            {
                options.Authority = authority;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                };
            })
            .AddJwtBearer(AuthScheme.Site, options =>
            {
                options.Authority = authority;
                options.Audience = siteAudience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicy.AllowEndUserPolicy, policy =>
            {
                policy.AuthenticationSchemes.Add(AuthScheme.Site);
                policy.RequireClaim(JwtRegisteredClaimNames.Sub);
            });
            options.AddPolicy(AuthPolicy.AllowClientPolicy, policy =>
            {
                policy.AuthenticationSchemes.Add(AuthScheme.Internal);
                policy.Requirements.Add(new DenyAnonymousAuthorizationRequirement());
                policy.Requirements.Add(new ScopeRequirement());
            });
        });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        return services;
    }
}