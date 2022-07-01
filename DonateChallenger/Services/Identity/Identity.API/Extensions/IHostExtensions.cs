using System;
using System.Threading.Tasks;
using Identity.API.Data;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.API.Extensions
{
    public static class IHostExtensions
    {
        public static void CreateDbIfNotExist<TDbContext>(this IApplicationBuilder app, IDbInitializer<TDbContext> dbInitializer)
            where TDbContext : DbContext
        {
            using var scope = app.ApplicationServices.CreateScope();
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

        private static async Task RetryActionOnFailureWithDelay(Func<Task> action, int delayInMilliseconds = 1000)
        {
            try
            {
                await action.Invoke();
            }
            catch
            {
                await Task.Delay(delayInMilliseconds);
                await RetryActionOnFailureWithDelay(async () => await action.Invoke());
            }
        }
    }
}