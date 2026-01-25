using FluentAssertions;
using Professional.Core.Entities;
using Xunit;

namespace Professional.Api.Tests;

public class ProfessionalTests
{
    private readonly Guid _tenantId = Guid.NewGuid();

    [Fact]
    public void Create_ValidInput_CreatesProfessional()
    {
        // Arrange & Act
        var professional = new Core.Entities.Professional(
            _tenantId,
            "Auto Repair Shop",
            "John",
            "Doe",
            "contact@autorepair.com");

        // Assert
        professional.Id.Should().NotBeEmpty();
        professional.TenantId.Should().Be(_tenantId);
        professional.BusinessName.Should().Be("Auto Repair Shop");
        professional.FirstName.Should().Be("John");
        professional.LastName.Should().Be("Doe");
        professional.Email.Should().Be("contact@autorepair.com");
        professional.Status.Should().Be(Shared.Contracts.Enums.ProfessionalStatus.Pending);
        professional.IsVerified.Should().BeFalse();
    }

    [Fact]
    public void Create_EmptyTenantId_ThrowsException()
    {
        // Arrange & Act
        var act = () => new Core.Entities.Professional(
            Guid.Empty,
            "Auto Shop",
            "John",
            "Doe",
            "test@test.com");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Tenant*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_EmptyBusinessName_ThrowsException(string businessName)
    {
        // Arrange & Act
        var act = () => new Core.Entities.Professional(
            _tenantId,
            businessName,
            "John",
            "Doe",
            "test@test.com");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*business*");
    }

    [Fact]
    public void Verify_PendingProfessional_VerifiesProfessional()
    {
        // Arrange
        var professional = new Core.Entities.Professional(
            _tenantId,
            "Auto Shop",
            "John",
            "Doe",
            "test@test.com");

        // Act
        professional.Verify();

        // Assert
        professional.IsVerified.Should().BeTrue();
        professional.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Activate_PendingProfessional_ActivatesProfessional()
    {
        // Arrange
        var professional = new Core.Entities.Professional(
            _tenantId,
            "Auto Shop",
            "John",
            "Doe",
            "test@test.com");

        // Act
        professional.Activate();

        // Assert
        professional.Status.Should().Be(Shared.Contracts.Enums.ProfessionalStatus.Active);
    }

    [Fact]
    public void AddSpecialty_ValidSpecialty_AddsSpecialty()
    {
        // Arrange
        var professional = new Core.Entities.Professional(
            _tenantId,
            "Auto Shop",
            "John",
            "Doe",
            "test@test.com");
        var specialtyId = Guid.NewGuid();

        // Act
        professional.AddSpecialty(specialtyId, "Engine Repair", "ASE Certified", 5);

        // Assert
        professional.Specialties.Should().HaveCount(1);
        professional.Specialties.First().SpecialtyId.Should().Be(specialtyId);
        professional.Specialties.First().SpecialtyName.Should().Be("Engine Repair");
    }

    [Fact]
    public void UpdateReferralSettings_ValidSettings_UpdatesSettings()
    {
        // Arrange
        var professional = new Core.Entities.Professional(
            _tenantId,
            "Auto Shop",
            "John",
            "Doe",
            "test@test.com");

        // Act
        professional.UpdateReferralSettings(true, 10.5m);

        // Assert
        professional.AcceptsReferrals.Should().BeTrue();
        professional.ReferralCommissionRate.Should().Be(10.5m);
    }
}
