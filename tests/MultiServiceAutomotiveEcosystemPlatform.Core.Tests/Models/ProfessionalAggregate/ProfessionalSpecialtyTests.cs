using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Models.ProfessionalAggregate;

public class ProfessionalSpecialtyTests
{
    private readonly Guid _professionalId = Guid.NewGuid();
    private readonly Guid _specialtyId = Guid.NewGuid();

    [Fact]
    public void Constructor_WithSpecialtyId_CreatesSpecialty()
    {
        // Act
        var specialty = new ProfessionalSpecialty(
            _professionalId,
            _specialtyId);

        // Assert
        Assert.NotEqual(Guid.Empty, specialty.ProfessionalSpecialtyId);
        Assert.Equal(_professionalId, specialty.ProfessionalId);
        Assert.Equal(_specialtyId, specialty.SpecialtyId);
        Assert.Null(specialty.CustomName);
        Assert.False(specialty.Verified);
    }

    [Fact]
    public void Constructor_WithCustomName_CreatesSpecialty()
    {
        // Act
        var specialty = new ProfessionalSpecialty(
            _professionalId,
            customName: "Custom Service",
            description: "A custom specialty",
            yearsExperience: 5,
            displayOrder: 1);

        // Assert
        Assert.Equal("Custom Service", specialty.CustomName);
        Assert.Equal("A custom specialty", specialty.Description);
        Assert.Equal(5, specialty.YearsExperience);
        Assert.Equal(1, specialty.DisplayOrder);
    }

    [Fact]
    public void Constructor_WithoutSpecialtyIdOrCustomName_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ProfessionalSpecialty(_professionalId));
    }

    [Fact]
    public void UpdateDetails_UpdatesDescriptionAndExperience()
    {
        // Arrange
        var specialty = new ProfessionalSpecialty(_professionalId, customName: "Service");

        // Act
        specialty.UpdateDetails("Updated description", 10);

        // Assert
        Assert.Equal("Updated description", specialty.Description);
        Assert.Equal(10, specialty.YearsExperience);
    }

    [Fact]
    public void UpdateCertification_SetsCertificationFields()
    {
        // Arrange
        var specialty = new ProfessionalSpecialty(_professionalId, customName: "Service");
        var certDate = new DateTime(2020, 1, 1);
        var expiryDate = new DateTime(2025, 1, 1);

        // Act
        specialty.UpdateCertification(
            "ASE Certified",
            "ASE",
            certDate,
            expiryDate,
            "https://cert.pdf");

        // Assert
        Assert.Equal("ASE Certified", specialty.CertificationName);
        Assert.Equal("ASE", specialty.CertificationIssuer);
        Assert.Equal(certDate, specialty.CertificationDate);
        Assert.Equal(expiryDate, specialty.CertificationExpiry);
        Assert.Equal("https://cert.pdf", specialty.CertificationDocumentUrl);
    }

    [Fact]
    public void UpdateDisplayOrder_SetsDisplayOrder()
    {
        // Arrange
        var specialty = new ProfessionalSpecialty(_professionalId, customName: "Service");

        // Act
        specialty.UpdateDisplayOrder(5);

        // Assert
        Assert.Equal(5, specialty.DisplayOrder);
    }

    [Fact]
    public void Verify_SetsVerifiedAndTimestamp()
    {
        // Arrange
        var specialty = new ProfessionalSpecialty(_professionalId, customName: "Service");
        var verifiedBy = Guid.NewGuid();

        // Act
        specialty.Verify(verifiedBy);

        // Assert
        Assert.True(specialty.Verified);
        Assert.NotNull(specialty.VerifiedAt);
        Assert.Equal(verifiedBy, specialty.VerifiedBy);
    }

    [Fact]
    public void Unverify_ClearsVerification()
    {
        // Arrange
        var specialty = new ProfessionalSpecialty(_professionalId, customName: "Service");
        specialty.Verify(Guid.NewGuid());

        // Act
        specialty.Unverify();

        // Assert
        Assert.False(specialty.Verified);
        Assert.Null(specialty.VerifiedAt);
        Assert.Null(specialty.VerifiedBy);
    }

    [Fact]
    public void IsCertificationExpired_WithExpiredDate_ReturnsTrue()
    {
        // Arrange
        var specialty = new ProfessionalSpecialty(_professionalId, customName: "Service");
        specialty.UpdateCertification("Cert", "Issuer", DateTime.UtcNow.AddYears(-2), DateTime.UtcNow.AddDays(-1), null);

        // Assert
        Assert.True(specialty.IsCertificationExpired);
    }

    [Fact]
    public void IsCertificationExpired_WithFutureDate_ReturnsFalse()
    {
        // Arrange
        var specialty = new ProfessionalSpecialty(_professionalId, customName: "Service");
        specialty.UpdateCertification("Cert", "Issuer", DateTime.UtcNow, DateTime.UtcNow.AddYears(1), null);

        // Assert
        Assert.False(specialty.IsCertificationExpired);
    }

    [Fact]
    public void IsCertificationExpired_WithNoExpiry_ReturnsFalse()
    {
        // Arrange
        var specialty = new ProfessionalSpecialty(_professionalId, customName: "Service");

        // Assert
        Assert.False(specialty.IsCertificationExpired);
    }
}
