using ChallengesTemporaryStorage.API.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Filters;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddCustomConfiguredSwagger("ChallengesTemporaryStorage", configuration, GetScopes())
    .AddConfiguredMessageBus(configuration)
    .AddCustomAuthorization(configuration)
    .AddAppDependencies()
    .AddAppCors()
    .RegisterAppConfigurations(configuration)
    .AddControllers(o => o.Filters.Add(typeof(HttpGlobalExceptionFilter)))
    .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true);

var app = builder.Build();

app.UseCustomConfiguredSwaggerWithUI(configuration, "ChallengesTemporaryStorage", "challengestemporarystorageswaggerui");
app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

Dictionary<string, string> GetScopes() => new Dictionary<string, string>
{
    { "challenges-temporary-storage.bff", "challenges-temporary-storage.bff" }
};