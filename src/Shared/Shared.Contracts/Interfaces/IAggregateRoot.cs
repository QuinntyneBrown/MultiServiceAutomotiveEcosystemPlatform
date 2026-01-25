namespace Shared.Contracts.Interfaces;

public interface IAggregateRoot : IEntity
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
}
