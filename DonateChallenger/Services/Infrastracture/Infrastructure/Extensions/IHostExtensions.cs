using Microsoft.Extensions.Hosting;

namespace Infrastructure.Extensions;

public static class IHostExtensions
{
    public static void CreateDbIfNotExist<TDbContext>(this IHost app, IDbInitializer<TDbContext> dbInitializer)
        where TDbContext : DbContext
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<TDbContext>();

            dbInitializer.Initialize(context).Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex} An error occurred creating the DB.");
        }
    }
}