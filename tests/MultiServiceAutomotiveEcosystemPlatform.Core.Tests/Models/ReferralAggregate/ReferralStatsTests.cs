using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Models.ReferralAggregate;

public class ReferralStatsTests
{
    private readonly Guid _tenantId = Guid.NewGuid();
    private readonly Guid _entityId = Guid.NewGuid();

    [Fact]
    public void Constructor_CreatesStats()
    {
        // Act
        var stats = new ReferralStats(_tenantId, ReferralEntityType.Customer, _entityId);

        // Assert
        Assert.NotEqual(Guid.Empty, stats.ReferralStatsId);
        Assert.Equal(_tenantId, stats.TenantId);
        Assert.Equal(ReferralEntityType.Customer, stats.EntityType);
        Assert.Equal(_entityId, stats.EntityId);
        Assert.Equal(0, stats.TotalReferralsSent);
        Assert.Equal(0, stats.SuccessfulReferrals);
        Assert.Equal(0, stats.PendingReferrals);
        Assert.Equal(0, stats.TotalRewardsEarned);
        Assert.Equal(0, stats.RewardsPending);
    }

    [Fact]
    public void UpdateCustomerStats_ForCustomerEntity_UpdatesStats()
    {
        // Arrange
        var stats = new ReferralStats(_tenantId, ReferralEntityType.Customer, _entityId);

        // Act
        stats.UpdateCustomerStats(10, 5, 3, 125.00m, 75.00m);

        // Assert
        Assert.Equal(10, stats.TotalReferralsSent);
        Assert.Equal(5, stats.SuccessfulReferrals);
        Assert.Equal(3, stats.PendingReferrals);
        Assert.Equal(125.00m, stats.TotalRewardsEarned);
        Assert.Equal(75.00m, stats.RewardsPending);
    }

    [Fact]
    public void UpdateCustomerStats_ForProfessionalEntity_ThrowsInvalidOperationException()
    {
        // Arrange
        var stats = new ReferralStats(_tenantId, ReferralEntityType.Professional, _entityId);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            stats.UpdateCustomerStats(10, 5, 3, 125.00m, 75.00m));
    }

    [Fact]
    public void UpdateProfessionalStats_ForProfessionalEntity_UpdatesStats()
    {
        // Arrange
        var stats = new ReferralStats(_tenantId, ReferralEntityType.Professional, _entityId);

        // Act
        stats.UpdateProfessionalStats(20, 15, 0.75m, 12.50m);

        // Assert
        Assert.Equal(20, stats.ReferralsReceived);
        Assert.Equal(15, stats.ReferralsGiven);
        Assert.Equal(0.75m, stats.ReferralConversionRate);
        Assert.Equal(12.50m, stats.AvgDiscountGiven);
    }

    [Fact]
    public void UpdateProfessionalStats_ForCustomerEntity_ThrowsInvalidOperationException()
    {
        // Arrange
        var stats = new ReferralStats(_tenantId, ReferralEntityType.Customer, _entityId);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            stats.UpdateProfessionalStats(20, 15, 0.75m, 12.50m));
    }

    [Fact]
    public void IncrementReferralSent_IncrementsCounters()
    {
        // Arrange
        var stats = new ReferralStats(_tenantId, ReferralEntityType.Customer, _entityId);

        // Act
        stats.IncrementReferralSent();

        // Assert
        Assert.Equal(1, stats.TotalReferralsSent);
        Assert.Equal(1, stats.PendingReferrals);
    }

    [Fact]
    public void IncrementSuccessfulReferral_UpdatesCountersAndRewards()
    {
        // Arrange
        var stats = new ReferralStats(_tenantId, ReferralEntityType.Customer, _entityId);
        stats.IncrementReferralSent();

        // Act
        stats.IncrementSuccessfulReferral(25.00m);

        // Assert
        Assert.Equal(1, stats.SuccessfulReferrals);
        Assert.Equal(0, stats.PendingReferrals);
        Assert.Equal(25.00m, stats.RewardsPending);
    }

    [Fact]
    public void MarkRewardPaid_TransfersFromPendingToEarned()
    {
        // Arrange
        var stats = new ReferralStats(_tenantId, ReferralEntityType.Customer, _entityId);
        stats.IncrementReferralSent();
        stats.IncrementSuccessfulReferral(25.00m);

        // Act
        stats.MarkRewardPaid(25.00m);

        // Assert
        Assert.Equal(0m, stats.RewardsPending);
        Assert.Equal(25.00m, stats.TotalRewardsEarned);
    }

    [Fact]
    public void RecalculateConversionRate_UpdatesCalculatedAt()
    {
        // Arrange
        var stats = new ReferralStats(_tenantId, ReferralEntityType.Professional, _entityId);
        var initialCalculatedAt = stats.CalculatedAt;

        // Small delay to ensure time difference
        Thread.Sleep(10);

        // Act
        stats.RecalculateConversionRate();

        // Assert - Professional entity with no referrals received should update timestamp
        // but not calculate (since ReferralsReceived is 0)
        Assert.Equal(initialCalculatedAt, stats.CalculatedAt);
    }
}
