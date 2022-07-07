using ChallengeOrder.API.Data;
using ChallengeOrder.API.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Filters;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddConfiguredMessageBus(configuration)
    .AddAppCors()
    .AddAppDependencies()
    .AddDbContextFactory<AppDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("OrderConnectionString")))
    .AddCustomConfiguredSwagger("ChallengeOrder", configuration, GetScopes())
    .AddControllers(o => o.Filters.Add(typeof(HttpGlobalExceptionFilter)))
    .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true);

var app = builder.Build();

app.UseCustomConfiguredSwaggerWithUI(configuration, "ChallengeOrder", "challengeorderswaggerui");

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.CreateDbIfNotExist(new AppDbContextInitializer());
app.Run();

Dictionary<string, string> GetScopes() => new Dictionary<string, string>
{
    { "challengeOrder", "challengeOrder" }
};