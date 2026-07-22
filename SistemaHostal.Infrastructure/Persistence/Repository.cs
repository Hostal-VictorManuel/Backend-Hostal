using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Common;

namespace SistemaHostal.Infrastructure.Persistence;

public abstract class Repository<TAggregate>(SistemaHostalDbContext context) : IRepository<TAggregate>
    where TAggregate : class, IAggregateRoot
{
    protected readonly SistemaHostalDbContext Context = context;

    public virtual async Task<TAggregate?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await Context.Set<TAggregate>().FindAsync([id], cancellationToken);

    public async Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        => await Context.Set<TAggregate>().AddAsync(aggregate, cancellationToken);

    public void Update(TAggregate aggregate) => Context.Set<TAggregate>().Update(aggregate);

    public void Remove(TAggregate aggregate) => Context.Set<TAggregate>().Remove(aggregate);
}