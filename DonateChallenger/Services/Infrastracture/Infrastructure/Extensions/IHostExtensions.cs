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
            RetryActionOnFailureWithDelay(async () => await dbInitializer.Initialize(context)).Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex} An error occurred creating the DB.");
        }
    }

    private static async Task RetryActionOnFailureWithDelay(Func<Task> action, int delayInMilliseconds = 5000)
    {
        try
        {
            Console.WriteLine($"CreateDbIfNotExist ---> I am trying to initialize database ");
            await action.Invoke();
        }
        catch
        {
            await Task.Delay(delayInMilliseconds);
            await RetryActionOnFailureWithDelay(async () => await action.Invoke());
        }
    }
}