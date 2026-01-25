namespace Shared.Contracts.Interfaces;

public interface IEntity
{
    Guid Id { get; }
}

public interface IEntity<TKey>
{
    TKey Id { get; }
}
