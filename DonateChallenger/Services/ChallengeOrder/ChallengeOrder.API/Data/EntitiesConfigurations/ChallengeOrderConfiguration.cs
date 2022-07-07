using ChallengeOrder.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChallengeOrder.API.Data.EntitiesConfigurations;

public class ChallengeOrderConfiguration : IEntityTypeConfiguration<ChallengeOrderEntity>
{
    public void Configure(EntityTypeBuilder<ChallengeOrderEntity> builder)
    {
        builder.ToTable("ChallengeOrder").HasKey(c => c.ChallengeOrderId);
        builder.Property(c => c.ChallengeOrderId).IsRequired();
        builder.Property(c => c.PaymentId).IsRequired();
        builder.Property(c => c.CatalogChallengeId).IsRequired();
    }
}