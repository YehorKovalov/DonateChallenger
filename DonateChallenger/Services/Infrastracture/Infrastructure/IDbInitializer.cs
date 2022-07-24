namespace Infrastructure;

public interface IDbInitializer<in TDbContext>
    where TDbContext : DbContext
{
    Task Initialize(TDbContext dbContext);
}