using ChallengeCatalog.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChallengeCatalog.API.Data.EntitiesConfigurations;

public class ChallengeConfiguration : IEntityTypeConfiguration<ChallengeEntity>
{
    public void Configure(EntityTypeBuilder<ChallengeEntity> builder)
    {
        builder.ToTable("Challenge").HasKey(c => c.ChallengeId);

        builder.Property(c => c.ChallengeId).UseHiLo("catalog_challenge_hilo").IsRequired();

        builder.Property(c => c.Description).HasMaxLength(50000).IsRequired();

        builder.Property(c => c.Title).HasMaxLength(30).IsRequired(false);

        builder.Property(c => c.DonateFrom).IsRequired();

        builder.Property(c => c.StreamerId).IsRequired();

        builder.Property(c => c.DonatePrice).IsRequired();

        builder.Property(c => c.CreatedTime).IsRequired();

        builder.HasOne(c => c.ChallengeStatusEntity)
            .WithOne(c => c.Challenge)
            .HasForeignKey<ChallengeStatusEntity>(c => c.StatusId);
    }
}