using Infrastructure.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class BaseDataService<TDbContext>
    where TDbContext : DbContext
{
    private readonly IDbContextWrapper<TDbContext> _dbContext;
    private readonly ILogger<BaseDataService<TDbContext>> _logger;

    public BaseDataService(
        IDbContextWrapper<TDbContext> dbContext,
        ILogger<BaseDataService<TDbContext>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    protected ILogger<BaseDataService<TDbContext>> Logger => _logger;
    protected Task ExecuteSafeAsync(
        Func<Task> action,
        CancellationToken cancellationToken = default)
    {
        return ExecuteSafeAsync(token => action(), cancellationToken);
    }

    protected Task<TResult> ExecuteSafeAsync<TResult>(
        Func<Task<TResult>> action,
        CancellationToken cancellationToken = default)
    {
        return ExecuteSafeAsync(token => action(), cancellationToken);
    }

    private async Task ExecuteSafeAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        try
        {
            await action(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"{nameof(ExecuteSafeAsync)} ---> transaction is rollbacked");
            throw;
        }
    }

    private async Task<TResult> ExecuteSafeAsync<TResult>(
        Func<CancellationToken, Task<TResult>> action,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await action(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, $"{nameof(ExecuteSafeAsync)} ---> transaction is rollbacked");
            throw;
        }
    }
}