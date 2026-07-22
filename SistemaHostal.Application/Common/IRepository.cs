using SistemaHostal.Domain.Common;

namespace SistemaHostal.Application.Common;

public interface IRepository<TAggregate> where TAggregate : class, IAggregateRoot
{
    Task<TAggregate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
    void Update(TAggregate aggregate);
    void Remove(TAggregate aggregate);
}