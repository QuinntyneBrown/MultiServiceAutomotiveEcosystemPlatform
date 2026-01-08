using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Models.ProfessionalAggregate;

public class SpecialtyCatalogTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesSpecialtyCatalog()
    {
        // Act
        var catalog = new SpecialtyCatalog(
            "Oil Change",
            "Maintenance",
            description: "Standard oil change service",
            icon: "oil-icon");

        // Assert
        Assert.NotEqual(Guid.Empty, catalog.SpecialtyCatalogId);
        Assert.Equal("Oil Change", catalog.Name);
        Assert.Equal("oil-change", catalog.Slug);
        Assert.Equal("Maintenance", catalog.Category);
        Assert.Equal("Standard oil change service", catalog.Description);
        Assert.Equal("oil-icon", catalog.Icon);
        Assert.True(catalog.IsActive);
        Assert.Null(catalog.TenantId);
        Assert.True(catalog.IsGlobal);
    }

    [Fact]
    public void Constructor_WithTenantId_SetsNonGlobal()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        // Act
        var catalog = new SpecialtyCatalog("Service", "Category", tenantId);

        // Assert
        Assert.Equal(tenantId, catalog.TenantId);
        Assert.False(catalog.IsGlobal);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyName_ThrowsArgumentException(string name)
    {
        Assert.Throws<ArgumentException>(() => new SpecialtyCatalog(name, "Category"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyCategory_ThrowsArgumentException(string category)
    {
        Assert.Throws<ArgumentException>(() => new SpecialtyCatalog("Name", category));
    }

    [Fact]
    public void UpdateDetails_WithValidParameters_UpdatesDetails()
    {
        // Arrange
        var catalog = new SpecialtyCatalog("Original", "Original Category");

        // Act
        catalog.UpdateDetails("Updated", "Updated Category", "New description", "new-icon");

        // Assert
        Assert.Equal("Updated", catalog.Name);
        Assert.Equal("Updated Category", catalog.Category);
        Assert.Equal("New description", catalog.Description);
        Assert.Equal("new-icon", catalog.Icon);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UpdateDetails_WithEmptyName_ThrowsArgumentException(string name)
    {
        // Arrange
        var catalog = new SpecialtyCatalog("Original", "Category");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => catalog.UpdateDetails(name, "Category", null, null));
    }

    [Fact]
    public void Activate_SetsIsActiveToTrue()
    {
        // Arrange
        var catalog = new SpecialtyCatalog("Name", "Category");
        catalog.Deactivate();

        // Act
        catalog.Activate();

        // Assert
        Assert.True(catalog.IsActive);
    }

    [Fact]
    public void Deactivate_SetsIsActiveToFalse()
    {
        // Arrange
        var catalog = new SpecialtyCatalog("Name", "Category");

        // Act
        catalog.Deactivate();

        // Assert
        Assert.False(catalog.IsActive);
    }

    [Fact]
    public void Constructor_GeneratesSlugFromName()
    {
        // Act
        var catalog = new SpecialtyCatalog("Brake Pad Replacement", "Repairs");

        // Assert
        Assert.Equal("brake-pad-replacement", catalog.Slug);
    }
}
