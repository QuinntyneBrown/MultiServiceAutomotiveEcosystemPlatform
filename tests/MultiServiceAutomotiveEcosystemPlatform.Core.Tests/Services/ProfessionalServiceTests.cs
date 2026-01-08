using Moq;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate.Enums;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;
using MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Helpers;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Services;

public class ProfessionalServiceTests
{
    private readonly Mock<IMultiServiceAutomotiveEcosystemPlatformContext> _mockContext;
    private readonly TestTenantContext _tenantContext;
    private readonly ProfessionalService _service;
    private readonly List<Professional> _professionals;
    private readonly List<ProfessionalSpecialty> _specialties;
    private readonly List<SpecialtyCatalog> _specialtyCatalogs;

    public ProfessionalServiceTests()
    {
        _mockContext = new Mock<IMultiServiceAutomotiveEcosystemPlatformContext>();
        _tenantContext = new TestTenantContext();
        _professionals = new List<Professional>();
        _specialties = new List<ProfessionalSpecialty>();
        _specialtyCatalogs = new List<SpecialtyCatalog>();

        var mockProfessionalsDbSet = MockDbSetHelper.CreateMockDbSet(_professionals);
        var mockSpecialtiesDbSet = MockDbSetHelper.CreateMockDbSet(_specialties);
        var mockCatalogDbSet = MockDbSetHelper.CreateMockDbSet(_specialtyCatalogs);

        _mockContext.Setup(c => c.Professionals).Returns(mockProfessionalsDbSet.Object);
        _mockContext.Setup(c => c.ProfessionalSpecialties).Returns(mockSpecialtiesDbSet.Object);
        _mockContext.Setup(c => c.SpecialtyCatalogs).Returns(mockCatalogDbSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _service = new ProfessionalService(_mockContext.Object, _tenantContext);
    }

    [Fact]
    public async Task CreateProfessionalAsync_WithValidParameters_CreatesProfessional()
    {
        // Act
        var professional = await _service.CreateProfessionalAsync(
            Guid.NewGuid(),
            "John's Auto",
            BusinessType.MechanicDomestic,
            "John",
            "Smith",
            "john@example.com",
            "1234567890",
            "123 Main St",
            "Boston",
            "MA",
            "02101");

        // Assert
        Assert.NotNull(professional);
        Assert.Equal("John's Auto", professional.BusinessName);
        Assert.Equal(_tenantContext.TenantId, professional.TenantId);
        Assert.Single(_professionals);
    }

    [Fact]
    public async Task GetProfessionalByIdAsync_WithExistingProfessional_ReturnsProfessional()
    {
        // Arrange
        var professional = await CreateTestProfessional();

        // Act
        var result = await _service.GetProfessionalByIdAsync(professional.ProfessionalId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(professional.ProfessionalId, result.ProfessionalId);
    }

    [Fact]
    public async Task GetProfessionalBySlugAsync_WithExistingSlug_ReturnsProfessional()
    {
        // Arrange
        var professional = await CreateTestProfessional();

        // Act
        var result = await _service.GetProfessionalBySlugAsync(professional.Slug);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(professional.Slug, result.Slug);
    }

    [Fact]
    public async Task GetActiveProfessionalsAsync_ReturnsOnlyActiveProfessionals()
    {
        // Arrange
        var prof1 = await CreateTestProfessional("Business 1", "business1@example.com");
        var prof2 = await CreateTestProfessional("Business 2", "business2@example.com");
        prof1.Activate();
        // prof2 remains in Pending status

        // Act
        var result = await _service.GetActiveProfessionalsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(prof1.ProfessionalId, result.First().ProfessionalId);
    }

    [Fact]
    public async Task GetProfessionalsByTypeAsync_ReturnsMatchingProfessionals()
    {
        // Arrange
        var mechanic = await _service.CreateProfessionalAsync(
            Guid.NewGuid(), "Mechanic Shop", BusinessType.MechanicDomestic,
            "John", "Smith", "mechanic@example.com", "1234567890",
            "123 Main St", "Boston", "MA", "02101");
        mechanic.Activate();

        var bodyShop = await _service.CreateProfessionalAsync(
            Guid.NewGuid(), "Body Shop", BusinessType.AutoBody,
            "Jane", "Doe", "bodyshop@example.com", "0987654321",
            "456 Oak St", "Boston", "MA", "02101");
        bodyShop.Activate();

        // Act
        var result = await _service.GetProfessionalsByTypeAsync(BusinessType.MechanicDomestic);

        // Assert
        Assert.Single(result);
        Assert.Equal("Mechanic Shop", result.First().BusinessName);
    }

    [Fact]
    public async Task GetFeaturedProfessionalsAsync_ReturnsOnlyFeatured()
    {
        // Arrange
        var prof1 = await CreateTestProfessional("Featured Business", "featured@example.com");
        var prof2 = await CreateTestProfessional("Regular Business", "regular@example.com");
        prof1.Activate();
        prof1.SetFeatured(true);
        prof2.Activate();

        // Act
        var result = await _service.GetFeaturedProfessionalsAsync();

        // Assert
        Assert.Single(result);
        Assert.True(result.First().Featured);
    }

    [Fact]
    public async Task GetProfessionalsAcceptingReferralsAsync_ReturnsCorrectProfessionals()
    {
        // Arrange
        var prof1 = await CreateTestProfessional("Accepts Referrals", "accepts@example.com");
        var prof2 = await CreateTestProfessional("No Referrals", "norefers@example.com");
        prof1.Activate();
        prof2.Activate();
        prof2.UpdateSettings(false, false, null);

        // Act
        var result = await _service.GetProfessionalsAcceptingReferralsAsync();

        // Assert
        Assert.Single(result);
        Assert.True(result.First().AcceptsReferrals);
    }

    [Fact]
    public async Task UpdateProfessionalAsync_UpdatesProfessional()
    {
        // Arrange
        var professional = await CreateTestProfessional();

        // Act
        var updated = await _service.UpdateProfessionalAsync(
            professional.ProfessionalId,
            "New Business Name",
            BusinessType.AutoBody,
            "Jane",
            "Doe");

        // Assert
        Assert.Equal("New Business Name", updated.BusinessName);
        Assert.Equal(BusinessType.AutoBody, updated.BusinessType);
        Assert.Equal("Jane", updated.FirstName);
    }

    [Fact]
    public async Task ActivateProfessionalAsync_ActivatesProfessional()
    {
        // Arrange
        var professional = await CreateTestProfessional();

        // Act
        await _service.ActivateProfessionalAsync(professional.ProfessionalId);

        // Assert
        Assert.True(professional.IsActive);
    }

    [Fact]
    public async Task SuspendProfessionalAsync_SuspendsProfessional()
    {
        // Arrange
        var professional = await CreateTestProfessional();
        professional.Activate();

        // Act
        await _service.SuspendProfessionalAsync(professional.ProfessionalId);

        // Assert
        Assert.Equal(ProfessionalStatus.Suspended, professional.Status);
    }

    [Fact]
    public async Task VerifyProfessionalAsync_VerifiesProfessional()
    {
        // Arrange
        var professional = await CreateTestProfessional();

        // Act
        await _service.VerifyProfessionalAsync(professional.ProfessionalId);

        // Assert
        Assert.True(professional.Verified);
        Assert.NotNull(professional.VerifiedAt);
    }

    [Fact]
    public async Task SetFeaturedAsync_SetsFeaturedFlag()
    {
        // Arrange
        var professional = await CreateTestProfessional();

        // Act
        await _service.SetFeaturedAsync(professional.ProfessionalId, true);

        // Assert
        Assert.True(professional.Featured);
    }

    [Fact]
    public async Task AddSpecialtyAsync_AddsSpecialtyToProfessional()
    {
        // Arrange
        var professional = await CreateTestProfessional();

        // Act
        var specialty = await _service.AddSpecialtyAsync(
            professional.ProfessionalId,
            null,
            "Oil Changes",
            "Fast oil changes",
            5);

        // Assert
        Assert.NotNull(specialty);
        Assert.Equal("Oil Changes", specialty.CustomName);
        Assert.Single(professional.Specialties);
    }

    [Fact]
    public async Task RemoveSpecialtyAsync_RemovesSpecialtyFromProfessional()
    {
        // Arrange
        var professional = await CreateTestProfessional();
        var specialty = await _service.AddSpecialtyAsync(
            professional.ProfessionalId,
            null,
            "Oil Changes",
            "Fast oil changes",
            5);

        // Act
        await _service.RemoveSpecialtyAsync(professional.ProfessionalId, specialty.ProfessionalSpecialtyId);

        // Assert
        Assert.Empty(professional.Specialties);
    }

    [Fact]
    public async Task GetSpecialtyCatalogAsync_ReturnsActiveSpecialties()
    {
        // Arrange
        _specialtyCatalogs.Add(new SpecialtyCatalog("Oil Change", "Maintenance"));
        _specialtyCatalogs.Add(new SpecialtyCatalog("Brake Service", "Repairs"));
        var inactive = new SpecialtyCatalog("Inactive Service", "Other");
        inactive.Deactivate();
        _specialtyCatalogs.Add(inactive);

        // Act
        var result = await _service.GetSpecialtyCatalogAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SlugExistsAsync_WithExistingSlug_ReturnsTrue()
    {
        // Arrange
        var professional = await CreateTestProfessional();

        // Act
        var result = await _service.SlugExistsAsync(professional.Slug);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SlugExistsAsync_WithExcludedProfessional_ReturnsFalse()
    {
        // Arrange
        var professional = await CreateTestProfessional();

        // Act
        var result = await _service.SlugExistsAsync(professional.Slug, professional.ProfessionalId);

        // Assert
        Assert.False(result);
    }

    private async Task<Professional> CreateTestProfessional(
        string businessName = "Test Business",
        string email = "test@example.com")
    {
        return await _service.CreateProfessionalAsync(
            Guid.NewGuid(),
            businessName,
            BusinessType.MechanicDomestic,
            "John",
            "Smith",
            email,
            "1234567890",
            "123 Main St",
            "Boston",
            "MA",
            "02101");
    }
}
