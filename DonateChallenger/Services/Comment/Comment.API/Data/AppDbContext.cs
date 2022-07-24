using Comment.API.Data.Entities;
using Comment.API.Data.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Comment.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<CommentEntity> Comments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
    }
}