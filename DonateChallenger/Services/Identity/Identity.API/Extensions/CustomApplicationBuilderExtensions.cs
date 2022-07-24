using Identity.API.Data;
using Microsoft.AspNetCore.Builder;

namespace Identity.API.Extensions;

public static class CustomApplicationBuilderExtensions
{
    public static IApplicationBuilder EnsureInitializeAppDbContexts(this IApplicationBuilder app)
    {
        app.CreateDbIfNotExist(new AppDbInitializer());
        app.CreateDbIfNotExist(new ConfigurationDbContextInitializer());
        app.CreateDbIfNotExist(new PersistedDbContextInitializer());
        return app;
    }
}