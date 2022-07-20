using Comment.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comment.API.Data.EntitiesConfigurations;

public class CommentConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.ToTable("Comment").HasKey(s => s.CommentId);

        builder.Property(s => s.CommentId).UseHiLo("comment_hilo").IsRequired();

        builder.Property(s => s.Message).HasMaxLength(2000).IsRequired();

        builder.Property(s => s.ChallengeId).IsRequired();

        builder.Property(s => s.Date).IsRequired();

        builder.Property(s => s.UserId).IsRequired();
    }
}