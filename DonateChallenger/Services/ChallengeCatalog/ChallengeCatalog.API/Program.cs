using ChallengeCatalog.API.Data;
using ChallengeCatalog.API.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContexts(builder.Configuration)
    .AddCustomConfiguredSwagger("ChallengeCatalog", builder.Configuration, GetScopes())
    .AddCustomAuthorization(builder.Configuration)
    .AddAppDependencies()
    .AddAppCors()
    .AddAutoMapper(typeof(Program))
    .AddControllers(o => o.Filters.Add(typeof(HttpGlobalExceptionFilter)))
    .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true);

var app = builder.Build();

app.UseCustomConfiguredSwaggerWithUI(builder.Configuration, "ChallengeCatalog", "challengecatalogswaggerui");

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.CreateDbIfNotExist(new ChallengesCatalogDbInitializer());
app.Run();

Dictionary<string, string> GetScopes() => new Dictionary<string, string>
{
    { "challengeCatalog", "challengeCatalog" }
};