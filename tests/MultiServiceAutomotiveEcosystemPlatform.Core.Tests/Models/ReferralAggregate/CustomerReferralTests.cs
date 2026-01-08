using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Models.ReferralAggregate;

public class CustomerReferralTests
{
    private readonly Guid _tenantId = Guid.NewGuid();
    private readonly Guid _referrerCustomerId = Guid.NewGuid();

    [Fact]
    public void Constructor_WithValidEmail_CreatesReferral()
    {
        // Act
        var referral = new CustomerReferral(
            _tenantId,
            _referrerCustomerId,
            "REFCODE1",
            refereeEmail: "friend@example.com",
            refereeName: "Friend Name");

        // Assert
        Assert.NotEqual(Guid.Empty, referral.CustomerReferralId);
        Assert.Equal(_tenantId, referral.TenantId);
        Assert.Equal(_referrerCustomerId, referral.ReferrerCustomerId);
        Assert.Equal("REFCODE1", referral.ReferrerCode);
        Assert.Equal("friend@example.com", referral.RefereeEmail);
        Assert.Equal("Friend Name", referral.RefereeName);
        Assert.Equal(CustomerReferralStatus.Pending, referral.Status);
        Assert.Equal(RewardStatus.Pending, referral.RewardStatus);
        Assert.True(referral.IsPending);
        Assert.False(referral.IsConverted);
        Assert.Null(referral.RefereeCustomerId);
    }

    [Fact]
    public void Constructor_WithValidPhone_CreatesReferral()
    {
        // Act
        var referral = new CustomerReferral(
            _tenantId,
            _referrerCustomerId,
            "REFCODE1",
            refereePhone: "1234567890");

        // Assert
        Assert.Equal("1234567890", referral.RefereePhone);
    }

    [Fact]
    public void Constructor_WithoutEmailOrPhone_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new CustomerReferral(
            _tenantId, _referrerCustomerId, "CODE"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyCode_ThrowsArgumentException(string code)
    {
        Assert.Throws<ArgumentException>(() => new CustomerReferral(
            _tenantId, _referrerCustomerId, code, refereeEmail: "test@example.com"));
    }

    [Fact]
    public void Constructor_SetsExpirationDate()
    {
        // Act
        var referral = new CustomerReferral(
            _tenantId, _referrerCustomerId, "CODE",
            refereeEmail: "test@example.com",
            expirationDays: 30);

        // Assert
        Assert.NotNull(referral.ExpiresAt);
        Assert.True(referral.ExpiresAt > DateTime.UtcNow.AddDays(29));
        Assert.True(referral.ExpiresAt < DateTime.UtcNow.AddDays(31));
    }

    [Fact]
    public void MarkContacted_FromPending_ChangesStatus()
    {
        // Arrange
        var referral = CreateReferral();

        // Act
        referral.MarkContacted();

        // Assert
        Assert.Equal(CustomerReferralStatus.Contacted, referral.Status);
        Assert.True(referral.IsPending);
    }

    [Fact]
    public void MarkContacted_FromNonPending_ThrowsInvalidOperationException()
    {
        // Arrange
        var referral = CreateReferral();
        referral.Convert(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => referral.MarkContacted());
    }

    [Fact]
    public void Convert_WithValidParameters_ConvertsReferral()
    {
        // Arrange
        var referral = CreateReferral();
        var refereeId = Guid.NewGuid();

        // Act
        referral.Convert(refereeId, 25.00m, RewardType.Cash);

        // Assert
        Assert.Equal(CustomerReferralStatus.Converted, referral.Status);
        Assert.Equal(refereeId, referral.RefereeCustomerId);
        Assert.Equal(25.00m, referral.RewardAmount);
        Assert.Equal(RewardType.Cash, referral.RewardType);
        Assert.NotNull(referral.ConvertedAt);
        Assert.True(referral.IsConverted);
        Assert.False(referral.IsPending);
    }

    [Fact]
    public void Convert_WhenAlreadyConverted_ThrowsInvalidOperationException()
    {
        // Arrange
        var referral = CreateReferral();
        referral.Convert(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => referral.Convert(Guid.NewGuid()));
    }

    [Fact]
    public void Convert_WhenExpired_ThrowsInvalidOperationException()
    {
        // Arrange
        var referral = CreateReferral();
        referral.Expire();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => referral.Convert(Guid.NewGuid()));
    }

    [Fact]
    public void ApproveReward_FromConverted_ApprovesReward()
    {
        // Arrange
        var referral = CreateReferral();
        referral.Convert(Guid.NewGuid());

        // Act
        referral.ApproveReward();

        // Assert
        Assert.Equal(RewardStatus.Approved, referral.RewardStatus);
    }

    [Fact]
    public void ApproveReward_WhenNotConverted_ThrowsInvalidOperationException()
    {
        // Arrange
        var referral = CreateReferral();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => referral.ApproveReward());
    }

    [Fact]
    public void MarkRewardPaid_FromApproved_MarksPaid()
    {
        // Arrange
        var referral = CreateReferral();
        referral.Convert(Guid.NewGuid());
        referral.ApproveReward();

        // Act
        referral.MarkRewardPaid();

        // Assert
        Assert.Equal(RewardStatus.Paid, referral.RewardStatus);
        Assert.NotNull(referral.RewardPaidAt);
    }

    [Fact]
    public void MarkRewardPaid_WhenNotApproved_ThrowsInvalidOperationException()
    {
        // Arrange
        var referral = CreateReferral();
        referral.Convert(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => referral.MarkRewardPaid());
    }

    [Fact]
    public void CancelReward_SetsRewardStatusToCancelled()
    {
        // Arrange
        var referral = CreateReferral();

        // Act
        referral.CancelReward();

        // Assert
        Assert.Equal(RewardStatus.Cancelled, referral.RewardStatus);
    }

    [Fact]
    public void Expire_FromPending_SetsStatusToExpired()
    {
        // Arrange
        var referral = CreateReferral();

        // Act
        referral.Expire();

        // Assert
        Assert.Equal(CustomerReferralStatus.Expired, referral.Status);
    }

    [Fact]
    public void Expire_WhenConverted_ThrowsInvalidOperationException()
    {
        // Arrange
        var referral = CreateReferral();
        referral.Convert(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => referral.Expire());
    }

    [Fact]
    public void Cancel_FromPending_CancelsReferral()
    {
        // Arrange
        var referral = CreateReferral();

        // Act
        referral.Cancel();

        // Assert
        Assert.Equal(CustomerReferralStatus.Cancelled, referral.Status);
        Assert.Equal(RewardStatus.Cancelled, referral.RewardStatus);
    }

    [Fact]
    public void Cancel_WhenConverted_ThrowsInvalidOperationException()
    {
        // Arrange
        var referral = CreateReferral();
        referral.Convert(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => referral.Cancel());
    }

    [Fact]
    public void SetTrackingInfo_SetsUtmParameters()
    {
        // Arrange
        var referral = CreateReferral();

        // Act
        referral.SetTrackingInfo("summer-campaign", "facebook");

        // Assert
        Assert.Equal("summer-campaign", referral.UtmCampaign);
        Assert.Equal("facebook", referral.UtmSource);
    }

    private CustomerReferral CreateReferral()
    {
        return new CustomerReferral(
            _tenantId,
            _referrerCustomerId,
            "REFCODE1",
            refereeEmail: "friend@example.com");
    }
}
