using Infrastructure.Filters;
using Microsoft.OpenApi.Models;

namespace Infrastructure.Extensions;

public static class SwaggerConfigurationExtension
{
    public static IServiceCollection AddCustomConfiguredSwagger(this IServiceCollection services, string serviceName)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            throw new ArgumentException();
        }

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"Donate-Challenger {serviceName} HTTP API",
                Version = "v1",
                Description = $"The {serviceName} Service HTTP API"
            });
        });

        return services;
    }

    public static IServiceCollection AddCustomConfiguredSwagger(this IServiceCollection services, string serviceName, IConfiguration config, Dictionary<string, string> scopes)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"Donate-Challenger {serviceName} HTTP API",
                Version = "v1",
                Description = $"The {serviceName} Service HTTP API"
            });

            if (scopes == null || config == null)
            {
                throw new ArgumentException();
            }

            var authority = config["Authorization:Authority"] ?? throw new ArgumentNullException();

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                        TokenUrl = new Uri($"{authority}/connect/token"),
                        Scopes = scopes
                    }
                }
            });

            options.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        return services;
    }

    public static IApplicationBuilder UseCustomConfiguredSwaggerWithUI(
        this WebApplication app,
        IConfiguration config,
        string serviceName,
        string? oAuthClientId = null)
    {
        if (config == null || string.IsNullOrWhiteSpace(serviceName))
        {
            throw new ArgumentException();
        }

        app.UseSwagger()
            .UseSwaggerUI(setup =>
            {
                var pathBase = config["PathBase"] ?? throw new ArgumentException();
                setup.SwaggerEndpoint($"{pathBase}/swagger/v1/swagger.json", $"{serviceName}.API V1");
                if (!string.IsNullOrWhiteSpace(oAuthClientId))
                {
                    setup.OAuthClientId(oAuthClientId);
                    setup.OAuthAppName($"{serviceName} Swagger UI");
                }
            });

        return app;
    }
}