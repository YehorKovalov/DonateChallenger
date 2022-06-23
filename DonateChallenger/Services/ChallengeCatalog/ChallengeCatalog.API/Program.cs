using ChallengeCatalog.API.Data;
using ChallengeCatalog.API.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDbContexts(builder.Configuration)
    .AddConfiguredSwagger("ChallengeCatalog")
    .AddAppDependencies()
    .AddAppCors()
    .AddAutoMapper(typeof(Program))
    .AddControllers(o => o.Filters.Add(typeof(HttpGlobalExceptionFilter)))
    .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true);

var app = builder.Build();

app.UseConfiguredSwaggerWithUI(builder.Configuration, "ChallengeCatalog");

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.CreateDbIfNotExist(new ChallengesCatalogDbInitializer());
app.Run();