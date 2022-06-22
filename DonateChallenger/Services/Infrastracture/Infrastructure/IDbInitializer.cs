namespace Infrastructure;

public interface IDbInitializer<in TDbContext>
    where TDbContext : DbContext
{
    void Initialize(TDbContext dbContext);
}