using Comment.API.Extensions;
using Infrastructure.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContexts(builder.Configuration)
    .AddControllers(o => o.Filters.Add(typeof(HttpGlobalExceptionFilter)))
    .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true);

var app = builder.Build();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();