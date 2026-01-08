using Moq;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;
using MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Helpers;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Services;

public class ReferralServiceTests
{
    private readonly Mock<IMultiServiceAutomotiveEcosystemPlatformContext> _mockContext;
    private readonly TestTenantContext _tenantContext;
    private readonly Mock<IReferralCodeGenerator> _mockCodeGenerator;
    private readonly ReferralService _service;
    private readonly List<CustomerReferral> _customerReferrals;
    private readonly List<ProfessionalReferral> _professionalReferrals;
    private readonly List<ReferralCode> _referralCodes;
    private readonly List<ReferralStats> _referralStats;

    public ReferralServiceTests()
    {
        _mockContext = new Mock<IMultiServiceAutomotiveEcosystemPlatformContext>();
        _tenantContext = new TestTenantContext();
        _mockCodeGenerator = new Mock<IReferralCodeGenerator>();
        _customerReferrals = new List<CustomerReferral>();
        _professionalReferrals = new List<ProfessionalReferral>();
        _referralCodes = new List<ReferralCode>();
        _referralStats = new List<ReferralStats>();

        SetupMocks();

        _service = new ReferralService(_mockContext.Object, _tenantContext, _mockCodeGenerator.Object);
    }

    private void SetupMocks()
    {
        var mockCustomerReferralsDbSet = MockDbSetHelper.CreateMockDbSet(_customerReferrals);
        var mockProfessionalReferralsDbSet = MockDbSetHelper.CreateMockDbSet(_professionalReferrals);
        var mockReferralCodesDbSet = MockDbSetHelper.CreateMockDbSet(_referralCodes);
        var mockReferralStatsDbSet = MockDbSetHelper.CreateMockDbSet(_referralStats);

        _mockContext.Setup(c => c.CustomerReferrals).Returns(mockCustomerReferralsDbSet.Object);
        _mockContext.Setup(c => c.ProfessionalReferrals).Returns(mockProfessionalReferralsDbSet.Object);
        _mockContext.Setup(c => c.ReferralCodes).Returns(mockReferralCodesDbSet.Object);
        _mockContext.Setup(c => c.ReferralStats).Returns(mockReferralStatsDbSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _mockCodeGenerator.Setup(g => g.GenerateCode(It.IsAny<string>())).Returns(() => "CODE" + Guid.NewGuid().ToString("N")[..4].ToUpper());
        _mockCodeGenerator.Setup(g => g.GenerateDiscountCode()).Returns(() => "DISC" + Guid.NewGuid().ToString("N")[..6].ToUpper());
    }

    #region Customer Referral Tests

    [Fact]
    public async Task CreateCustomerReferralAsync_WithValidEmail_CreatesReferral()
    {
        // Arrange
        var referrerId = Guid.NewGuid();

        // Act
        var referral = await _service.CreateCustomerReferralAsync(
            referrerId,
            "friend@example.com",
            null,
            "Friend Name");

        // Assert
        Assert.NotNull(referral);
        Assert.Equal("friend@example.com", referral.RefereeEmail);
        Assert.Equal(CustomerReferralStatus.Pending, referral.Status);
        Assert.Single(_customerReferrals);
    }

    [Fact]
    public async Task CreateCustomerReferralAsync_CreatesReferralCodeIfNotExists()
    {
        // Arrange
        var referrerId = Guid.NewGuid();

        // Act
        await _service.CreateCustomerReferralAsync(referrerId, "friend@example.com", null, null);

        // Assert
        Assert.Single(_referralCodes);
        Assert.Equal(referrerId, _referralCodes[0].CustomerId);
    }

    [Fact]
    public async Task CreateCustomerReferralAsync_WithDuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var referrerId = Guid.NewGuid();
        await _service.CreateCustomerReferralAsync(referrerId, "friend@example.com", null, null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateCustomerReferralAsync(referrerId, "friend@example.com", null, null));
    }

    [Fact]
    public async Task GetCustomerReferralByIdAsync_WithExistingReferral_ReturnsReferral()
    {
        // Arrange
        var referral = await _service.CreateCustomerReferralAsync(
            Guid.NewGuid(), "friend@example.com", null, null);

        // Act
        var result = await _service.GetCustomerReferralByIdAsync(referral.CustomerReferralId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(referral.CustomerReferralId, result.CustomerReferralId);
    }

    [Fact]
    public async Task GetCustomerReferralsByReferrerAsync_ReturnsReferrals()
    {
        // Arrange
        var referrerId = Guid.NewGuid();
        await _service.CreateCustomerReferralAsync(referrerId, "friend1@example.com", null, null);
        await _service.CreateCustomerReferralAsync(referrerId, "friend2@example.com", null, null);
        await _service.CreateCustomerReferralAsync(Guid.NewGuid(), "friend3@example.com", null, null);

        // Act
        var result = await _service.GetCustomerReferralsByReferrerAsync(referrerId);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ConvertCustomerReferralAsync_ConvertsReferral()
    {
        // Arrange
        var referral = await _service.CreateCustomerReferralAsync(
            Guid.NewGuid(), "friend@example.com", null, null);
        var refereeId = Guid.NewGuid();

        // Act
        var converted = await _service.ConvertCustomerReferralAsync(
            referral.CustomerReferralId, refereeId, 25.00m);

        // Assert
        Assert.Equal(CustomerReferralStatus.Converted, converted.Status);
        Assert.Equal(refereeId, converted.RefereeCustomerId);
        Assert.Equal(25.00m, converted.RewardAmount);
    }

    [Fact]
    public async Task ApproveRewardAsync_ApprovesReward()
    {
        // Arrange
        var referral = await _service.CreateCustomerReferralAsync(
            Guid.NewGuid(), "friend@example.com", null, null);
        await _service.ConvertCustomerReferralAsync(referral.CustomerReferralId, Guid.NewGuid());

        // Act
        await _service.ApproveRewardAsync(referral.CustomerReferralId);

        // Assert
        Assert.Equal(RewardStatus.Approved, referral.RewardStatus);
    }

    [Fact]
    public async Task MarkRewardPaidAsync_MarksPaid()
    {
        // Arrange
        var referral = await _service.CreateCustomerReferralAsync(
            Guid.NewGuid(), "friend@example.com", null, null);
        await _service.ConvertCustomerReferralAsync(referral.CustomerReferralId, Guid.NewGuid(), 25m);
        await _service.ApproveRewardAsync(referral.CustomerReferralId);

        // Act
        await _service.MarkRewardPaidAsync(referral.CustomerReferralId);

        // Assert
        Assert.Equal(RewardStatus.Paid, referral.RewardStatus);
        Assert.NotNull(referral.RewardPaidAt);
    }

    [Fact]
    public async Task CancelCustomerReferralAsync_CancelsReferral()
    {
        // Arrange
        var referral = await _service.CreateCustomerReferralAsync(
            Guid.NewGuid(), "friend@example.com", null, null);

        // Act
        await _service.CancelCustomerReferralAsync(referral.CustomerReferralId);

        // Assert
        Assert.Equal(CustomerReferralStatus.Cancelled, referral.Status);
    }

    #endregion

    #region Professional Referral Tests

    [Fact]
    public async Task CreateProfessionalReferralAsync_CreatesReferral()
    {
        // Arrange
        var sourceId = Guid.NewGuid();
        var targetId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        // Act
        var referral = await _service.CreateProfessionalReferralAsync(
            sourceId, targetId, customerId,
            "Customer needs brake repair",
            "Brake Repair",
            "VIP customer",
            ReferralPriority.High);

        // Assert
        Assert.NotNull(referral);
        Assert.Equal(sourceId, referral.SourceProfessionalId);
        Assert.Equal(targetId, referral.TargetProfessionalId);
        Assert.Equal(customerId, referral.CustomerId);
        Assert.Equal(ReferralPriority.High, referral.Priority);
        Assert.Single(_professionalReferrals);
    }

    [Fact]
    public async Task CreateProfessionalReferralAsync_WithDiscount_SetsDiscount()
    {
        // Arrange
        var sourceId = Guid.NewGuid();
        var targetId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        // Act
        var referral = await _service.CreateProfessionalReferralAsync(
            sourceId, targetId, customerId,
            discountType: DiscountType.Percentage,
            discountValue: 10m);

        // Assert
        Assert.True(referral.DiscountOffered);
        Assert.Equal(DiscountType.Percentage, referral.DiscountType);
        Assert.Equal(10m, referral.DiscountValue);
        Assert.NotNull(referral.DiscountCode);
    }

    [Fact]
    public async Task GetSentProfessionalReferralsAsync_ReturnsReferrals()
    {
        // Arrange
        var sourceId = Guid.NewGuid();
        await _service.CreateProfessionalReferralAsync(sourceId, Guid.NewGuid(), Guid.NewGuid());
        await _service.CreateProfessionalReferralAsync(sourceId, Guid.NewGuid(), Guid.NewGuid());
        await _service.CreateProfessionalReferralAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = await _service.GetSentProfessionalReferralsAsync(sourceId);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetReceivedProfessionalReferralsAsync_ReturnsReferrals()
    {
        // Arrange
        var targetId = Guid.NewGuid();
        await _service.CreateProfessionalReferralAsync(Guid.NewGuid(), targetId, Guid.NewGuid());
        await _service.CreateProfessionalReferralAsync(Guid.NewGuid(), targetId, Guid.NewGuid());
        await _service.CreateProfessionalReferralAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = await _service.GetReceivedProfessionalReferralsAsync(targetId);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AcceptProfessionalReferralAsync_AcceptsReferral()
    {
        // Arrange
        var referral = await _service.CreateProfessionalReferralAsync(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // Act
        await _service.AcceptProfessionalReferralAsync(referral.ProfessionalReferralId);

        // Assert
        Assert.Equal(ProfessionalReferralStatus.Accepted, referral.Status);
        Assert.NotNull(referral.AcceptedAt);
    }

    [Fact]
    public async Task DeclineProfessionalReferralAsync_DeclinesReferral()
    {
        // Arrange
        var referral = await _service.CreateProfessionalReferralAsync(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // Act
        await _service.DeclineProfessionalReferralAsync(referral.ProfessionalReferralId, "Too busy");

        // Assert
        Assert.Equal(ProfessionalReferralStatus.Declined, referral.Status);
        Assert.Equal("Too busy", referral.DeclinedReason);
    }

    [Fact]
    public async Task CompleteProfessionalReferralAsync_CompletesReferral()
    {
        // Arrange
        var referral = await _service.CreateProfessionalReferralAsync(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        await _service.AcceptProfessionalReferralAsync(referral.ProfessionalReferralId);

        // Act
        await _service.CompleteProfessionalReferralAsync(referral.ProfessionalReferralId);

        // Assert
        Assert.Equal(ProfessionalReferralStatus.Completed, referral.Status);
        Assert.NotNull(referral.CompletedAt);
    }

    #endregion

    #region Referral Code Tests

    [Fact]
    public async Task CreateReferralCodeAsync_CreatesCode()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var code = await _service.CreateReferralCodeAsync(customerId);

        // Assert
        Assert.NotNull(code);
        Assert.Equal(customerId, code.CustomerId);
        Assert.Equal(ReferralCodeType.Customer, code.CodeType);
        Assert.Single(_referralCodes);
    }

    [Fact]
    public async Task CreateReferralCodeAsync_ReturnsSameCodeIfExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var firstCode = await _service.CreateReferralCodeAsync(customerId);

        // Act
        var secondCode = await _service.CreateReferralCodeAsync(customerId);

        // Assert
        Assert.Equal(firstCode.ReferralCodeId, secondCode.ReferralCodeId);
        Assert.Single(_referralCodes);
    }

    [Fact]
    public async Task GetReferralCodeByCodeAsync_ReturnsCode()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var code = await _service.CreateReferralCodeAsync(customerId);

        // Act
        var result = await _service.GetReferralCodeByCodeAsync(code.Code);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(code.Code, result.Code);
    }

    [Fact]
    public async Task ValidateReferralCodeAsync_WithValidCode_ReturnsTrue()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var code = await _service.CreateReferralCodeAsync(customerId);

        // Act
        var result = await _service.ValidateReferralCodeAsync(code.Code);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ValidateReferralCodeAsync_WithInvalidCode_ReturnsFalse()
    {
        // Act
        var result = await _service.ValidateReferralCodeAsync("INVALIDCODE");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IncrementCodeUsageAsync_IncrementsCounter()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var code = await _service.CreateReferralCodeAsync(customerId);

        // Act
        await _service.IncrementCodeUsageAsync(code.Code);

        // Assert
        Assert.Equal(1, code.CurrentUses);
    }

    #endregion

    #region Statistics Tests

    [Fact]
    public async Task GetReferralStatsAsync_ReturnsStats()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        await _service.UpdateReferralStatsAsync(ReferralEntityType.Customer, entityId);

        // Act
        var result = await _service.GetReferralStatsAsync(ReferralEntityType.Customer, entityId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityId, result.EntityId);
    }

    [Fact]
    public async Task UpdateReferralStatsAsync_CreatesStatsIfNotExists()
    {
        // Arrange
        var entityId = Guid.NewGuid();

        // Act
        var stats = await _service.UpdateReferralStatsAsync(ReferralEntityType.Customer, entityId);

        // Assert
        Assert.NotNull(stats);
        Assert.Equal(entityId, stats.EntityId);
        Assert.Single(_referralStats);
    }

    #endregion
}
