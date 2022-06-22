using Infrastructure.Services.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Services;

public class DbContextWrapper<TDbContext> : IDbContextWrapper<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public DbContextWrapper(IDbContextFactory<TDbContext> context) => _context = context.CreateDbContext();

    public TDbContext DbContext => _context;

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken) => await _context.Database.BeginTransactionAsync(cancellationToken);
}