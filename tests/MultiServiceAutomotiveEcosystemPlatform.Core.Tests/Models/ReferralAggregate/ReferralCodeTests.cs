using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Models.ReferralAggregate;

public class ReferralCodeTests
{
    private readonly Guid _tenantId = Guid.NewGuid();
    private readonly Guid _customerId = Guid.NewGuid();
    private readonly Guid _professionalId = Guid.NewGuid();
    private readonly Guid _campaignId = Guid.NewGuid();

    [Fact]
    public void Constructor_WithCustomerCode_CreatesCode()
    {
        // Act
        var code = new ReferralCode(
            _tenantId,
            "JOHN7K2M",
            ReferralCodeType.Customer,
            customerId: _customerId);

        // Assert
        Assert.NotEqual(Guid.Empty, code.ReferralCodeId);
        Assert.Equal(_tenantId, code.TenantId);
        Assert.Equal("JOHN7K2M", code.Code);
        Assert.Equal(ReferralCodeType.Customer, code.CodeType);
        Assert.Equal(_customerId, code.CustomerId);
        Assert.Null(code.ProfessionalId);
        Assert.Null(code.CampaignId);
        Assert.True(code.IsActive);
        Assert.Equal(0, code.CurrentUses);
        Assert.True(code.CanBeUsed);
    }

    [Fact]
    public void Constructor_WithProfessionalCode_RequiresProfessionalId()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ReferralCode(
            _tenantId, "CODE", ReferralCodeType.Professional));
    }

    [Fact]
    public void Constructor_WithCampaignCode_RequiresCampaignId()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ReferralCode(
            _tenantId, "CODE", ReferralCodeType.Campaign));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyCode_ThrowsArgumentException(string codeValue)
    {
        Assert.Throws<ArgumentException>(() => new ReferralCode(
            _tenantId, codeValue, ReferralCodeType.Customer, customerId: _customerId));
    }

    [Fact]
    public void Constructor_NormalizesCodeToUppercase()
    {
        // Act
        var code = new ReferralCode(_tenantId, "abc123", ReferralCodeType.Customer, customerId: _customerId);

        // Assert
        Assert.Equal("ABC123", code.Code);
    }

    [Fact]
    public void Constructor_WithMaxUses_SetsMaxUses()
    {
        // Act
        var code = new ReferralCode(
            _tenantId, "CODE", ReferralCodeType.Customer,
            customerId: _customerId, maxUses: 10);

        // Assert
        Assert.Equal(10, code.MaxUses);
        Assert.Equal(10, code.RemainingUses);
    }

    [Fact]
    public void Constructor_WithRewards_SetsRewardFields()
    {
        // Act
        var code = new ReferralCode(
            _tenantId, "CODE", ReferralCodeType.Customer,
            customerId: _customerId, rewardAmount: 25m, discountPercentage: 10m);

        // Assert
        Assert.Equal(25m, code.RewardAmount);
        Assert.Equal(10m, code.DiscountPercentage);
    }

    [Fact]
    public void IncrementUses_IncrementsCounter()
    {
        // Arrange
        var code = new ReferralCode(_tenantId, "CODE", ReferralCodeType.Customer, customerId: _customerId);

        // Act
        code.IncrementUses();

        // Assert
        Assert.Equal(1, code.CurrentUses);
    }

    [Fact]
    public void IncrementUses_WhenCannotBeUsed_ThrowsInvalidOperationException()
    {
        // Arrange
        var code = new ReferralCode(
            _tenantId, "CODE", ReferralCodeType.Customer,
            customerId: _customerId, maxUses: 1);
        code.IncrementUses();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => code.IncrementUses());
    }

    [Fact]
    public void UpdateConfiguration_UpdatesFields()
    {
        // Arrange
        var code = new ReferralCode(_tenantId, "CODE", ReferralCodeType.Customer, customerId: _customerId);

        // Act
        code.UpdateConfiguration(5, 30m, 15m);

        // Assert
        Assert.Equal(5, code.MaxUses);
        Assert.Equal(30m, code.RewardAmount);
        Assert.Equal(15m, code.DiscountPercentage);
    }

    [Fact]
    public void SetExpiration_SetsExpirationDate()
    {
        // Arrange
        var code = new ReferralCode(_tenantId, "CODE", ReferralCodeType.Customer, customerId: _customerId);
        var expiry = DateTime.UtcNow.AddDays(30);

        // Act
        code.SetExpiration(expiry);

        // Assert
        Assert.Equal(expiry, code.ExpiresAt);
    }

    [Fact]
    public void Activate_SetsIsActiveToTrue()
    {
        // Arrange
        var code = new ReferralCode(_tenantId, "CODE", ReferralCodeType.Customer, customerId: _customerId);
        code.Deactivate();

        // Act
        code.Activate();

        // Assert
        Assert.True(code.IsActive);
    }

    [Fact]
    public void Deactivate_SetsIsActiveToFalse()
    {
        // Arrange
        var code = new ReferralCode(_tenantId, "CODE", ReferralCodeType.Customer, customerId: _customerId);

        // Act
        code.Deactivate();

        // Assert
        Assert.False(code.IsActive);
        Assert.False(code.CanBeUsed);
    }

    [Fact]
    public void IsExpired_WithPastDate_ReturnsTrue()
    {
        // Arrange
        var code = new ReferralCode(
            _tenantId, "CODE", ReferralCodeType.Customer,
            customerId: _customerId, expiresAt: DateTime.UtcNow.AddDays(-1));

        // Assert
        Assert.True(code.IsExpired);
        Assert.False(code.CanBeUsed);
    }

    [Fact]
    public void IsExpired_WithFutureDate_ReturnsFalse()
    {
        // Arrange
        var code = new ReferralCode(
            _tenantId, "CODE", ReferralCodeType.Customer,
            customerId: _customerId, expiresAt: DateTime.UtcNow.AddDays(30));

        // Assert
        Assert.False(code.IsExpired);
    }

    [Fact]
    public void HasReachedMaxUses_WhenAtMax_ReturnsTrue()
    {
        // Arrange
        var code = new ReferralCode(
            _tenantId, "CODE", ReferralCodeType.Customer,
            customerId: _customerId, maxUses: 2);
        code.IncrementUses();
        code.IncrementUses();

        // Assert
        Assert.True(code.HasReachedMaxUses);
        Assert.False(code.CanBeUsed);
    }

    [Fact]
    public void RemainingUses_CalculatesCorrectly()
    {
        // Arrange
        var code = new ReferralCode(
            _tenantId, "CODE", ReferralCodeType.Customer,
            customerId: _customerId, maxUses: 5);
        code.IncrementUses();
        code.IncrementUses();

        // Assert
        Assert.Equal(3, code.RemainingUses);
    }

    [Fact]
    public void RemainingUses_WithUnlimitedUses_ReturnsNull()
    {
        // Arrange
        var code = new ReferralCode(_tenantId, "CODE", ReferralCodeType.Customer, customerId: _customerId);

        // Assert
        Assert.Null(code.RemainingUses);
    }
}
