using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Models.ReferralAggregate;

public class ProfessionalReferralTests
{
    private readonly Guid _tenantId = Guid.NewGuid();
    private readonly Guid _sourceProfessionalId = Guid.NewGuid();
    private readonly Guid _targetProfessionalId = Guid.NewGuid();
    private readonly Guid _customerId = Guid.NewGuid();

    [Fact]
    public void Constructor_WithValidParameters_CreatesReferral()
    {
        // Act
        var referral = new ProfessionalReferral(
            _tenantId,
            _sourceProfessionalId,
            _targetProfessionalId,
            _customerId,
            "Customer needs brake repair",
            "Brake Repair",
            "VIP customer",
            ReferralPriority.High);

        // Assert
        Assert.NotEqual(Guid.Empty, referral.ProfessionalReferralId);
        Assert.Equal(_tenantId, referral.TenantId);
        Assert.Equal(_sourceProfessionalId, referral.SourceProfessionalId);
        Assert.Equal(_targetProfessionalId, referral.TargetProfessionalId);
        Assert.Equal(_customerId, referral.CustomerId);
        Assert.Equal("Customer needs brake repair", referral.Reason);
        Assert.Equal("Brake Repair", referral.ServiceNeeded);
        Assert.Equal("VIP customer", referral.Notes);
        Assert.Equal(ReferralPriority.High, referral.Priority);
        Assert.Equal(ProfessionalReferralStatus.Pending, referral.Status);
        Assert.True(referral.IsPending);
        Assert.True(referral.IsActive);
    }

    [Fact]
    public void Constructor_WithSameSourceAndTarget_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new ProfessionalReferral(
            _tenantId, _sourceProfessionalId, _sourceProfessionalId, _customerId));
    }

    [Fact]
    public void Constructor_SetsExpirationDate()
    {
        // Act
        var referral = new ProfessionalReferral(
            _tenantId, _sourceProfessionalId, _targetProfessionalId, _customerId,
            expirationDays: 14);

        // Assert
        Assert.NotNull(referral.ExpiresAt);
        Assert.True(referral.ExpiresAt > DateTime.UtcNow.AddDays(13));
    }

    [Fact]
    public void SetDiscount_WithPercentage_SetsDiscountFields()
    {
        // Arrange
        var referral = CreateReferral();

        // Act
        referral.SetDiscount(DiscountType.Percentage, 10m, "DISC10");

        // Assert
        Assert.True(referral.DiscountOffered);
        Assert.Equal(DiscountType.Percentage, referral.DiscountType);
        Assert.Equal(10m, referral.DiscountValue);
        Assert.Equal("DISC10", referral.DiscountCode);
        Assert.False(referral.DiscountUsed);
    }

    [Fact]
    public void SetDiscount_WithNone_ClearsDiscount()
    {
        // Arrange
        var referral = CreateReferral();
        referral.SetDiscount(DiscountType.Percentage, 10m, "DISC10");

        // Act
        referral.SetDiscount(DiscountType.None, null, null);

        // Assert
        Assert.False(referral.DiscountOffered);
        Assert.Equal(DiscountType.None, referral.DiscountType);
    }

    [Fact]
    public void SetDiscount_WithTypeButNoValue_ThrowsArgumentException()
    {
        // Arrange
        var referral = CreateReferral();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => referral.SetDiscount(DiscountType.Percentage, null, null));
    }

    [Fact]
    public void Accept_FromPending_AcceptsReferral()
    {
        // Arrange
        var referral = CreateReferral();

        // Act
        referral.Accept();

        // Assert
        Assert.Equal(ProfessionalReferralStatus.Accepted, referral.Status);
        Assert.NotNull(referral.AcceptedAt);
        Assert.False(referral.IsPending);
        Assert.True(referral.IsActive);
    }

    [Fact]
    public void Accept_WhenNotPending_ThrowsInvalidOperationException()
    {
        // Arrange
        var referral = CreateReferral();
        referral.Accept();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => referral.Accept());
    }

    [Fact]
    public void Decline_FromPending_DeclinesReferral()
    {
        // Arrange
        var referral = CreateReferral();

        // Act
        referral.Decline("Too busy");

        // Assert
        Assert.Equal(ProfessionalReferralStatus.Declined, referral.Status);
        Assert.Equal("Too busy", referral.DeclinedReason);
        Assert.False(referral.IsActive);
    }

    [Fact]
    public void Complete_FromAccepted_CompletesReferral()
    {
        // Arrange
        var referral = CreateReferral();
        referral.Accept();

        // Act
        referral.Complete();

        // Assert
        Assert.Equal(ProfessionalReferralStatus.Completed, referral.Status);
        Assert.NotNull(referral.CompletedAt);
        Assert.True(referral.IsCompleted);
        Assert.False(referral.IsActive);
    }

    [Fact]
    public void Complete_WhenNotAccepted_ThrowsInvalidOperationException()
    {
        // Arrange
        var referral = CreateReferral();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => referral.Complete());
    }

    [Fact]
    public void Expire_FromPending_ExpiresReferral()
    {
        // Arrange
        var referral = CreateReferral();

        // Act
        referral.Expire();

        // Assert
        Assert.Equal(ProfessionalReferralStatus.Expired, referral.Status);
    }

    [Fact]
    public void Expire_WhenNotPending_ThrowsInvalidOperationException()
    {
        // Arrange
        var referral = CreateReferral();
        referral.Accept();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => referral.Expire());
    }

    [Fact]
    public void MarkDiscountUsed_WhenDiscountOffered_MarksUsed()
    {
        // Arrange
        var referral = CreateReferral();
        referral.SetDiscount(DiscountType.Fixed, 25m, "FIXED25");

        // Act
        referral.MarkDiscountUsed();

        // Assert
        Assert.True(referral.DiscountUsed);
    }

    [Fact]
    public void MarkDiscountUsed_WhenNoDiscountOffered_ThrowsInvalidOperationException()
    {
        // Arrange
        var referral = CreateReferral();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => referral.MarkDiscountUsed());
    }

    [Fact]
    public void SetFollowUp_SetsFollowUpFields()
    {
        // Arrange
        var referral = CreateReferral();
        var followUpDate = DateTime.UtcNow.AddDays(7);

        // Act
        referral.SetFollowUp(followUpDate, "Check in with customer");

        // Assert
        Assert.True(referral.FollowUpRequired);
        Assert.Equal(followUpDate, referral.FollowUpDate);
        Assert.Equal("Check in with customer", referral.FollowUpNotes);
    }

    [Fact]
    public void ClearFollowUp_ClearsFollowUpFields()
    {
        // Arrange
        var referral = CreateReferral();
        referral.SetFollowUp(DateTime.UtcNow.AddDays(7), "Notes");

        // Act
        referral.ClearFollowUp();

        // Assert
        Assert.False(referral.FollowUpRequired);
        Assert.Null(referral.FollowUpDate);
        Assert.Null(referral.FollowUpNotes);
    }

    [Fact]
    public void UpdateNotes_SetsNotes()
    {
        // Arrange
        var referral = CreateReferral();

        // Act
        referral.UpdateNotes("Updated notes");

        // Assert
        Assert.Equal("Updated notes", referral.Notes);
    }

    private ProfessionalReferral CreateReferral()
    {
        return new ProfessionalReferral(
            _tenantId,
            _sourceProfessionalId,
            _targetProfessionalId,
            _customerId);
    }
}
