using Comment.API.Data;
using Comment.API.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Filters;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
    .AddDbContexts(configuration)
    .AddAppCors()
    .AddAppDependencies()
    .AddConfiguredMessageBus(configuration)
    .AddCustomAuthorization(configuration)
    .AddCustomConfiguredSwagger("Comment", configuration, Scopes())
    .AddControllers(o => o.Filters.Add(typeof(HttpGlobalExceptionFilter)))
    .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true);

var app = builder.Build();

app.UseCustomConfiguredSwaggerWithUI(configuration, "Comment", "commentswaggerui");

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.CreateDbIfNotExist(new AppDbInitializer());
app.Run();

Dictionary<string, string> Scopes() => new Dictionary<string, string>
{
    { "comment.bff", "comment.bff" },
    { "comment.manager", "comment.manager" },
};