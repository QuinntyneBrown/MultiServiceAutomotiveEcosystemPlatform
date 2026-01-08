namespace MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;

public class CustomerOwnershipHistory
{
    public Guid CustomerOwnershipHistoryId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid? PreviousOwnerId { get; private set; }
    public Guid NewOwnerId { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public Guid TransferredBy { get; private set; }
    public DateTime TransferredAt { get; private set; }

    private CustomerOwnershipHistory() { }

    public CustomerOwnershipHistory(
        Guid tenantId,
        Guid customerId,
        Guid? previousOwnerId,
        Guid newOwnerId,
        string reason,
        Guid transferredBy)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason cannot be empty.", nameof(reason));
        if (previousOwnerId == newOwnerId)
            throw new ArgumentException("Previous owner and new owner cannot be the same.", nameof(newOwnerId));

        CustomerOwnershipHistoryId = Guid.NewGuid();
        TenantId = tenantId;
        CustomerId = customerId;
        PreviousOwnerId = previousOwnerId;
        NewOwnerId = newOwnerId;
        Reason = reason.Trim();
        TransferredBy = transferredBy;
        TransferredAt = DateTime.UtcNow;
    }
}
