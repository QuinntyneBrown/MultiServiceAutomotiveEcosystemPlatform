using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Models.ProfessionalAggregate;

public class ProfessionalTests
{
    private readonly Guid _tenantId = Guid.NewGuid();
    private readonly Guid _userId = Guid.NewGuid();

    [Fact]
    public void Constructor_WithValidParameters_CreatesProfessional()
    {
        // Act
        var professional = new Professional(
            _tenantId,
            _userId,
            "John's Auto Shop",
            BusinessType.MechanicDomestic,
            "John",
            "Smith",
            "john@example.com",
            "1234567890",
            "123 Main St",
            "Toronto",
            "ON",
            "M5H 2N2");

        // Assert
        Assert.NotEqual(Guid.Empty, professional.ProfessionalId);
        Assert.Equal(_tenantId, professional.TenantId);
        Assert.Equal(_userId, professional.UserId);
        Assert.Equal("John's Auto Shop", professional.BusinessName);
        Assert.Equal(BusinessType.MechanicDomestic, professional.BusinessType);
        Assert.Equal("John", professional.FirstName);
        Assert.Equal("Smith", professional.LastName);
        Assert.Equal("John Smith", professional.FullName);
        Assert.Equal("john@example.com", professional.Email);
        Assert.Equal("1234567890", professional.Phone);
        Assert.Equal(ProfessionalStatus.Pending, professional.Status);
        Assert.True(professional.AcceptsReferrals);
        Assert.False(professional.AutoAcceptInquiries);
        Assert.False(professional.Verified);
        Assert.False(professional.Featured);
        Assert.NotEmpty(professional.Slug);
    }

    [Fact]
    public void Constructor_GeneratesSlugFromBusinessName()
    {
        // Act
        var professional = new Professional(
            _tenantId,
            _userId,
            "John's Auto Shop",
            BusinessType.MechanicDomestic,
            "John",
            "Smith",
            "john@example.com",
            "1234567890",
            "123 Main St",
            "Toronto",
            "ON",
            "M5H 2N2");

        // Assert
        Assert.Equal("john-s-auto-shop", professional.Slug);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyBusinessName_ThrowsArgumentException(string businessName)
    {
        Assert.Throws<ArgumentException>(() => new Professional(
            _tenantId, _userId, businessName, BusinessType.MechanicDomestic,
            "John", "Smith", "john@example.com", "1234567890",
            "123 Main St", "Toronto", "ON", "M5H 2N2"));
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("missing@dot")]
    public void Constructor_WithInvalidEmail_ThrowsArgumentException(string email)
    {
        Assert.Throws<ArgumentException>(() => new Professional(
            _tenantId, _userId, "Business", BusinessType.MechanicDomestic,
            "John", "Smith", email, "1234567890",
            "123 Main St", "Toronto", "ON", "M5H 2N2"));
    }

    [Theory]
    [InlineData("123")]
    [InlineData("")]
    public void Constructor_WithInvalidPhone_ThrowsArgumentException(string phone)
    {
        Assert.Throws<ArgumentException>(() => new Professional(
            _tenantId, _userId, "Business", BusinessType.MechanicDomestic,
            "John", "Smith", "john@example.com", phone,
            "123 Main St", "Toronto", "ON", "M5H 2N2"));
    }

    [Fact]
    public void UpdateBusinessInfo_WithValidParameters_UpdatesInfo()
    {
        // Arrange
        var professional = CreateProfessional();

        // Act
        professional.UpdateBusinessInfo("New Business Name", BusinessType.AutoBody, "LIC123", "TAX456");

        // Assert
        Assert.Equal("New Business Name", professional.BusinessName);
        Assert.Equal(BusinessType.AutoBody, professional.BusinessType);
        Assert.Equal("LIC123", professional.LicenseNumber);
        Assert.Equal("TAX456", professional.TaxId);
    }

    [Fact]
    public void UpdatePersonalInfo_WithValidParameters_UpdatesInfo()
    {
        // Arrange
        var professional = CreateProfessional();

        // Act
        professional.UpdatePersonalInfo("Jane", "Doe", "Master Technician", "10 years experience");

        // Assert
        Assert.Equal("Jane", professional.FirstName);
        Assert.Equal("Doe", professional.LastName);
        Assert.Equal("Master Technician", professional.Title);
        Assert.Equal("10 years experience", professional.Bio);
    }

    [Fact]
    public void UpdateContactInfo_WithValidParameters_UpdatesInfo()
    {
        // Arrange
        var professional = CreateProfessional();

        // Act
        professional.UpdateContactInfo("new@example.com", "0987654321", "5555555555", "https://example.com");

        // Assert
        Assert.Equal("new@example.com", professional.Email);
        Assert.Equal("0987654321", professional.Phone);
        Assert.Equal("5555555555", professional.PhoneBusiness);
        Assert.Equal("https://example.com", professional.Website);
    }

    [Fact]
    public void UpdateAddress_WithValidParameters_UpdatesAddress()
    {
        // Arrange
        var professional = CreateProfessional();

        // Act
        professional.UpdateAddress("456 Oak St", "Suite 100", "Vancouver", "BC", "V5K 0A1");

        // Assert
        Assert.Equal("456 Oak St", professional.AddressLine1);
        Assert.Equal("Suite 100", professional.AddressLine2);
        Assert.Equal("Vancouver", professional.City);
        Assert.Equal("BC", professional.Province);
        Assert.Equal("V5K 0A1", professional.PostalCode);
    }

    [Fact]
    public void UpdateLocation_SetsLocationFields()
    {
        // Arrange
        var professional = CreateProfessional();

        // Act
        professional.UpdateLocation(42.3601m, -71.0589m, 25);

        // Assert
        Assert.Equal(42.3601m, professional.Latitude);
        Assert.Equal(-71.0589m, professional.Longitude);
        Assert.Equal(25, professional.ServiceRadiusMiles);
    }

    [Fact]
    public void UpdateMedia_SetsMediaUrls()
    {
        // Arrange
        var professional = CreateProfessional();

        // Act
        professional.UpdateMedia("https://profile.jpg", "https://cover.jpg", "https://logo.jpg");

        // Assert
        Assert.Equal("https://profile.jpg", professional.ProfilePhotoUrl);
        Assert.Equal("https://cover.jpg", professional.CoverPhotoUrl);
        Assert.Equal("https://logo.jpg", professional.LogoUrl);
    }

    [Fact]
    public void UpdateSettings_SetsSettings()
    {
        // Arrange
        var professional = CreateProfessional();

        // Act
        professional.UpdateSettings(false, true, "{\"email\": true}");

        // Assert
        Assert.False(professional.AcceptsReferrals);
        Assert.True(professional.AutoAcceptInquiries);
        Assert.Equal("{\"email\": true}", professional.NotificationPreferences);
    }

    [Fact]
    public void Activate_SetsStatusToActive()
    {
        // Arrange
        var professional = CreateProfessional();

        // Act
        professional.Activate();

        // Assert
        Assert.Equal(ProfessionalStatus.Active, professional.Status);
        Assert.True(professional.IsActive);
    }

    [Fact]
    public void Suspend_SetsStatusToSuspended()
    {
        // Arrange
        var professional = CreateProfessional();
        professional.Activate();

        // Act
        professional.Suspend();

        // Assert
        Assert.Equal(ProfessionalStatus.Suspended, professional.Status);
        Assert.False(professional.IsActive);
    }

    [Fact]
    public void Deactivate_SetsStatusToInactive()
    {
        // Arrange
        var professional = CreateProfessional();
        professional.Activate();

        // Act
        professional.Deactivate();

        // Assert
        Assert.Equal(ProfessionalStatus.Inactive, professional.Status);
        Assert.False(professional.IsActive);
    }

    [Fact]
    public void Verify_SetsVerifiedToTrueAndTimestamp()
    {
        // Arrange
        var professional = CreateProfessional();

        // Act
        professional.Verify();

        // Assert
        Assert.True(professional.Verified);
        Assert.NotNull(professional.VerifiedAt);
    }

    [Fact]
    public void SetFeatured_SetsFeatureFlag()
    {
        // Arrange
        var professional = CreateProfessional();

        // Act
        professional.SetFeatured(true);

        // Assert
        Assert.True(professional.Featured);
    }

    [Fact]
    public void AddSpecialty_AddsToCollection()
    {
        // Arrange
        var professional = CreateProfessional();
        var specialty = new ProfessionalSpecialty(professional.ProfessionalId, customName: "Oil Changes");

        // Act
        professional.AddSpecialty(specialty);

        // Assert
        Assert.Single(professional.Specialties);
        Assert.Contains(specialty, professional.Specialties);
    }

    [Fact]
    public void RemoveSpecialty_RemovesFromCollection()
    {
        // Arrange
        var professional = CreateProfessional();
        var specialty = new ProfessionalSpecialty(professional.ProfessionalId, customName: "Oil Changes");
        professional.AddSpecialty(specialty);

        // Act
        professional.RemoveSpecialty(specialty);

        // Assert
        Assert.Empty(professional.Specialties);
    }

    private Professional CreateProfessional()
    {
        return new Professional(
            _tenantId,
            _userId,
            "Test Auto Shop",
            BusinessType.MechanicDomestic,
            "John",
            "Smith",
            "john@example.com",
            "1234567890",
            "123 Main St",
            "Toronto",
            "ON",
            "M5H 2N2");
    }
}
