using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Services.Abstractions;

public interface IDbContextWrapper<out TDbContext>
    where TDbContext : DbContext
{
    TDbContext DbContext { get; }

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}