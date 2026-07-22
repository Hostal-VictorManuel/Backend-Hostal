using MediatR;

namespace SistemaHostal.Domain.Common;

public abstract record DomainEvent : INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}