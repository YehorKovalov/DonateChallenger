using ChallengeOrder.API.Data.Entities;
using ChallengeOrder.API.Data.EntitiesConfigurations;

namespace ChallengeOrder.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ChallengeOrderEntity> ChallengeOrders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChallengeOrderConfiguration());
    }
}