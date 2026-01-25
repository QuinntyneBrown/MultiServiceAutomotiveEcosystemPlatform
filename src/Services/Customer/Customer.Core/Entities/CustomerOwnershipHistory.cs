using Shared.Contracts.Interfaces;

namespace Customer.Core.Entities;

public class CustomerOwnershipHistory : IEntity
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid? PreviousOwnerId { get; private set; }
    public Guid NewOwnerId { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public DateTime TransferredAt { get; private set; }

    private CustomerOwnershipHistory() { }

    public CustomerOwnershipHistory(Guid customerId, Guid? previousOwnerId, Guid newOwnerId, string reason)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        PreviousOwnerId = previousOwnerId;
        NewOwnerId = newOwnerId;
        Reason = reason;
        TransferredAt = DateTime.UtcNow;
    }
}
