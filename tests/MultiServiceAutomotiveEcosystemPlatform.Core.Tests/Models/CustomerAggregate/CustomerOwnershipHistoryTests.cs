using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Models.CustomerAggregate;

public class CustomerOwnershipHistoryTests
{
    private readonly Guid _tenantId = Guid.NewGuid();
    private readonly Guid _customerId = Guid.NewGuid();
    private readonly Guid _previousOwnerId = Guid.NewGuid();
    private readonly Guid _newOwnerId = Guid.NewGuid();
    private readonly Guid _transferredBy = Guid.NewGuid();

    [Fact]
    public void Constructor_WithValidParameters_CreatesHistory()
    {
        // Act
        var history = new CustomerOwnershipHistory(
            _tenantId,
            _customerId,
            _previousOwnerId,
            _newOwnerId,
            "Customer requested transfer",
            _transferredBy);

        // Assert
        Assert.NotEqual(Guid.Empty, history.CustomerOwnershipHistoryId);
        Assert.Equal(_tenantId, history.TenantId);
        Assert.Equal(_customerId, history.CustomerId);
        Assert.Equal(_previousOwnerId, history.PreviousOwnerId);
        Assert.Equal(_newOwnerId, history.NewOwnerId);
        Assert.Equal("Customer requested transfer", history.Reason);
        Assert.Equal(_transferredBy, history.TransferredBy);
        Assert.True(history.TransferredAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Constructor_WithNullPreviousOwner_SetsNullPreviousOwnerId()
    {
        // Act
        var history = new CustomerOwnershipHistory(
            _tenantId,
            _customerId,
            null,
            _newOwnerId,
            "Initial assignment",
            _transferredBy);

        // Assert
        Assert.Null(history.PreviousOwnerId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyReason_ThrowsArgumentException(string reason)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CustomerOwnershipHistory(
            _tenantId,
            _customerId,
            _previousOwnerId,
            _newOwnerId,
            reason,
            _transferredBy));
    }

    [Fact]
    public void Constructor_WithSameOwner_ThrowsArgumentException()
    {
        // Arrange
        var sameId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CustomerOwnershipHistory(
            _tenantId,
            _customerId,
            sameId,
            sameId,
            "Transfer",
            _transferredBy));
    }
}
