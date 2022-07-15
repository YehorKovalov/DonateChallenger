using Infrastructure.Extensions;
using Infrastructure.Filters;
using Payment.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddCustomConfiguredSwagger("Payment", configuration, GetScopes())
    .AddCustomAuthorization(configuration)
    .AddAppDependencies()
    .AddConfiguredMessageBus(configuration)
    .AddAppCors()
    .RegisterAppConfigurations(configuration)
    .AddControllers(o => o.Filters.Add(typeof(HttpGlobalExceptionFilter)))
    .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true);

var app = builder.Build();

app.UseCustomConfiguredSwaggerWithUI(configuration, "Payment", "paymentswaggerui");
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

Dictionary<string, string> GetScopes() => new Dictionary<string, string>
{
    { "paypalPayment", "paypalPayment" }
};