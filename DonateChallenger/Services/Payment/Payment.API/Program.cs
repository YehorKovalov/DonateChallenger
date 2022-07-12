using Infrastructure.Extensions;
using Infrastructure.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Payment.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services
    .AddCustomConfiguredSwagger("Payment", config, GetScopes())
    .AddAppDependencies()
    .RegisterAppConfigurations(config)
    .AddControllers(o => o.Filters.Add(typeof(HttpGlobalExceptionFilter)))
    .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true);

var app = builder.Build();

app.UseCustomConfiguredSwaggerWithUI(config, "Payment", "paymentswaggerui");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

Dictionary<string, string> GetScopes() => new Dictionary<string, string>
{
    { "payment", "payment" }
};