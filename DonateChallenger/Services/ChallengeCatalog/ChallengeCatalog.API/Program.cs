using ChallengeCatalog.API.Data;
using ChallengeCatalog.API.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Filters;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
    .AddDbContexts(configuration)
    .AddConfiguredMessageBus(configuration)
    .AddCustomConfiguredSwagger("ChallengeCatalog", configuration, GetScopes())
    .AddCustomAuthorization(configuration)
    .AddAppDependencies()
    .AddAppCors()
    .AddAutoMapper(typeof(Program))
    .AddControllers(o => o.Filters.Add(typeof(HttpGlobalExceptionFilter)))
    .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true);

var app = builder.Build();

app.UseCustomConfiguredSwaggerWithUI(configuration, "ChallengeCatalog", "challengecatalogswaggerui");

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.CreateDbIfNotExist(new ChallengesCatalogDbInitializer());
app.Run();

Dictionary<string, string> GetScopes() => new Dictionary<string, string>
{
    { "challenge-catalog.bff", "challenge-catalog.bff" }
};