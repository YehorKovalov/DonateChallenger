using ChallengeCatalog.API.Data.Entities;
using ChallengeCatalog.API.Data.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace ChallengeCatalog.API.Data;

public class ChallengeCatalogDbContext : DbContext
{
    public ChallengeCatalogDbContext(DbContextOptions<ChallengeCatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<ChallengeEntity> Challenges { get; set; } = null!;

    public DbSet<ChallengeStatusEntity> Statuses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChallengeConfiguration());
        modelBuilder.ApplyConfiguration(new ChallengeStatusConfiguration());
    }
}