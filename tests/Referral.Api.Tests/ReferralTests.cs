using FluentAssertions;
using Referral.Core.Entities;
using Referral.Core.Services;
using Shared.Contracts.Enums;
using Xunit;

namespace Referral.Api.Tests;

public class CustomerReferralTests
{
    private readonly Guid _tenantId = Guid.NewGuid();
    private readonly Guid _referringCustomerId = Guid.NewGuid();

    [Fact]
    public void Create_ValidInput_CreatesCustomerReferral()
    {
        // Arrange & Act
        var referral = new CustomerReferral(
            _tenantId,
            _referringCustomerId,
            "referred@example.com",
            "555-1234",
            "Jane",
            "Doe",
            "Website",
            "REFCODE",
            "Friend recommendation");

        // Assert
        referral.Id.Should().NotBeEmpty();
        referral.TenantId.Should().Be(_tenantId);
        referral.ReferringCustomerId.Should().Be(_referringCustomerId);
        referral.ReferredEmail.Should().Be("referred@example.com");
        referral.Status.Should().Be(CustomerReferralStatus.Pending);
        referral.RewardStatus.Should().Be(RewardStatus.Pending);
    }

    [Fact]
    public void MarkContacted_PendingReferral_ChangesStatus()
    {
        // Arrange
        var referral = new CustomerReferral(
            _tenantId,
            _referringCustomerId,
            "referred@example.com",
            null, null, null, null, null, null);

        // Act
        referral.MarkContacted();

        // Assert
        referral.Status.Should().Be(CustomerReferralStatus.Contacted);
        referral.ContactedAt.Should().NotBeNull();
    }

    [Fact]
    public void Convert_ContactedReferral_ConvertsAndSetsReward()
    {
        // Arrange
        var referral = new CustomerReferral(
            _tenantId,
            _referringCustomerId,
            "referred@example.com",
            null, null, null, null, null, null);
        referral.MarkContacted();
        var newCustomerId = Guid.NewGuid();

        // Act
        referral.Convert(newCustomerId, 50m, "Cash");

        // Assert
        referral.Status.Should().Be(CustomerReferralStatus.Converted);
        referral.ConvertedCustomerId.Should().Be(newCustomerId);
        referral.ConvertedAt.Should().NotBeNull();
        referral.RewardAmount.Should().Be(50m);
        referral.RewardType.Should().Be("Cash");
        referral.RewardStatus.Should().Be(RewardStatus.Approved);
    }
}

public class ReferralCodeTests
{
    private readonly Guid _tenantId = Guid.NewGuid();
    private readonly Guid _customerId = Guid.NewGuid();

    [Fact]
    public void Create_ValidInput_CreatesReferralCode()
    {
        // Arrange & Act
        var code = ReferralCode.Create(
            _tenantId,
            "TESTCODE",
            _customerId,
            null,
            "Summer Campaign",
            DateTime.UtcNow.AddDays(30),
            100);

        // Assert
        code.Id.Should().NotBeEmpty();
        code.TenantId.Should().Be(_tenantId);
        code.Code.Should().Be("TESTCODE");
        code.CustomerId.Should().Be(_customerId);
        code.IsActive.Should().BeTrue();
        code.CurrentUses.Should().Be(0);
        code.MaxUses.Should().Be(100);
    }

    [Fact]
    public void IsValid_ActiveCodeWithinLimits_ReturnsTrue()
    {
        // Arrange
        var code = ReferralCode.Create(
            _tenantId,
            "TESTCODE",
            _customerId,
            null,
            null,
            DateTime.UtcNow.AddDays(30),
            100);

        // Act
        var isValid = code.IsValid();

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void IsValid_ExpiredCode_ReturnsFalse()
    {
        // Arrange
        var code = ReferralCode.Create(
            _tenantId,
            "TESTCODE",
            _customerId,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            100);

        // Act
        var isValid = code.IsValid();

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void RecordUsage_ValidCode_IncrementsUsage()
    {
        // Arrange
        var code = ReferralCode.Create(
            _tenantId,
            "TESTCODE",
            _customerId,
            null,
            null,
            null,
            null);

        // Act
        code.RecordUsage();

        // Assert
        code.CurrentUses.Should().Be(1);
    }
}

public class ReferralCodeGeneratorTests
{
    [Fact]
    public void Generate_DefaultLength_Returns8Characters()
    {
        // Act
        var code = ReferralCodeGenerator.Generate();

        // Assert
        code.Should().HaveLength(8);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(10)]
    [InlineData(16)]
    public void Generate_CustomLength_ReturnsCorrectLength(int length)
    {
        // Act
        var code = ReferralCodeGenerator.Generate(length);

        // Assert
        code.Should().HaveLength(length);
    }

    [Fact]
    public void Generate_MultipleCalls_ReturnsUniqueValues()
    {
        // Act
        var codes = Enumerable.Range(0, 100)
            .Select(_ => ReferralCodeGenerator.Generate())
            .ToList();

        // Assert
        codes.Distinct().Should().HaveCount(100);
    }
}
