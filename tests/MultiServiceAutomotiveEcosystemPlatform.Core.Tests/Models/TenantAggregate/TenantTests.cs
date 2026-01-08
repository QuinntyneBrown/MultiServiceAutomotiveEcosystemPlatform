using MultiServiceAutomotiveEcosystemPlatform.Core.Models.TenantAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.TenantAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Models.TenantAggregate;

public class TenantTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesTenant()
    {
        // Arrange
        var slug = "test-tenant";
        var name = "Test Tenant";
        var displayName = "Test Tenant Display";

        // Act
        var tenant = new Tenant(slug, name, displayName);

        // Assert
        Assert.NotEqual(Guid.Empty, tenant.TenantId);
        Assert.Equal("test-tenant", tenant.Slug);
        Assert.Equal("Test Tenant", tenant.Name);
        Assert.Equal("Test Tenant Display", tenant.DisplayName);
        Assert.Equal(TenantStatus.Active, tenant.Status);
        Assert.True(tenant.IsActive);
        Assert.Null(tenant.LogoUrl);
        Assert.Null(tenant.PrimaryColor);
        Assert.Null(tenant.Configuration);
    }

    [Fact]
    public void Constructor_WithOptionalParameters_SetsAllProperties()
    {
        // Arrange & Act
        var tenant = new Tenant(
            "test-tenant",
            "Test Tenant",
            "Test Tenant Display",
            "https://example.com/logo.png",
            "#FF0000",
            "{\"key\": \"value\"}");

        // Assert
        Assert.Equal("https://example.com/logo.png", tenant.LogoUrl);
        Assert.Equal("#FF0000", tenant.PrimaryColor);
        Assert.Equal("{\"key\": \"value\"}", tenant.Configuration);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptySlug_ThrowsArgumentException(string slug)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Tenant(slug, "Name", "Display"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyName_ThrowsArgumentException(string name)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Tenant("slug", name, "Display"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyDisplayName_ThrowsArgumentException(string displayName)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Tenant("slug", "Name", displayName));
    }

    [Fact]
    public void Constructor_NormalizesSlug()
    {
        // Arrange & Act
        var tenant = new Tenant("  TEST-TENANT  ", "Name", "Display");

        // Assert
        Assert.Equal("test-tenant", tenant.Slug);
    }

    [Fact]
    public void UpdateDetails_WithValidParameters_UpdatesProperties()
    {
        // Arrange
        var tenant = new Tenant("slug", "Original Name", "Original Display");
        var originalUpdatedAt = tenant.UpdatedAt;

        // Act
        tenant.UpdateDetails("New Name", "New Display", "https://new-logo.png", "#00FF00");

        // Assert
        Assert.Equal("New Name", tenant.Name);
        Assert.Equal("New Display", tenant.DisplayName);
        Assert.Equal("https://new-logo.png", tenant.LogoUrl);
        Assert.Equal("#00FF00", tenant.PrimaryColor);
        Assert.True(tenant.UpdatedAt >= originalUpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateDetails_WithEmptyName_ThrowsArgumentException(string name)
    {
        // Arrange
        var tenant = new Tenant("slug", "Name", "Display");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => tenant.UpdateDetails(name, "Display", null, null));
    }

    [Fact]
    public void UpdateConfiguration_SetsConfiguration()
    {
        // Arrange
        var tenant = new Tenant("slug", "Name", "Display");

        // Act
        tenant.UpdateConfiguration("{\"setting\": true}");

        // Assert
        Assert.Equal("{\"setting\": true}", tenant.Configuration);
    }

    [Fact]
    public void Activate_SetsStatusToActive()
    {
        // Arrange
        var tenant = new Tenant("slug", "Name", "Display");
        tenant.Suspend();

        // Act
        tenant.Activate();

        // Assert
        Assert.Equal(TenantStatus.Active, tenant.Status);
        Assert.True(tenant.IsActive);
    }

    [Fact]
    public void Suspend_SetsStatusToSuspended()
    {
        // Arrange
        var tenant = new Tenant("slug", "Name", "Display");

        // Act
        tenant.Suspend();

        // Assert
        Assert.Equal(TenantStatus.Suspended, tenant.Status);
        Assert.False(tenant.IsActive);
    }

    [Fact]
    public void Deactivate_SetsStatusToInactive()
    {
        // Arrange
        var tenant = new Tenant("slug", "Name", "Display");

        // Act
        tenant.Deactivate();

        // Assert
        Assert.Equal(TenantStatus.Inactive, tenant.Status);
        Assert.False(tenant.IsActive);
    }
}
