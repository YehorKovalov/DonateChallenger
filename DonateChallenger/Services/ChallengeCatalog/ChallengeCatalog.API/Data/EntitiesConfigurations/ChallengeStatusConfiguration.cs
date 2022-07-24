using ChallengeCatalog.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChallengeCatalog.API.Data.EntitiesConfigurations;

public class ChallengeStatusConfiguration : IEntityTypeConfiguration<ChallengeStatusEntity>
{
    public void Configure(EntityTypeBuilder<ChallengeStatusEntity> builder)
    {
        builder.ToTable("ChallengeStatus").HasKey(s => s.StatusId);

        builder.Property(s => s.StatusId).UseHiLo("catalog_challenge_hilo");

        builder.Property(s => s.IsCompleted).IsRequired();

        builder.Property(s => s.IsSkipped).IsRequired();
    }
}