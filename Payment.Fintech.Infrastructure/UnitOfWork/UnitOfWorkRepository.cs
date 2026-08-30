
namespace Payment.Fintech.Infrastructure.UnitOfWork;

internal class UnitOfWorkRepository(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if(_transaction is null)
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if(_transaction is not null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
