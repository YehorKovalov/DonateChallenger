using ChallengeCatalog.API.Data;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContextFactory<ChallengeCatalogDbContext>(o =>
{
    var connectionString = builder.Configuration.GetConnectionString("ChallengeConnectionString");
    o.UseNpgsql(connectionString);
});
builder.Services.AddScoped<IDbContextWrapper<ChallengeCatalogDbContext>, DbContextWrapper<ChallengeCatalogDbContext>>();

var app = builder.Build();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.CreateDbIfNotExist(new ChallengesCatalogDbInitializer());
app.Run();