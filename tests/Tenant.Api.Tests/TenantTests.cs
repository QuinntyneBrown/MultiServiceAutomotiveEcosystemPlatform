using FluentAssertions;
using Tenant.Core.Entities;
using Xunit;

namespace Tenant.Api.Tests;

public class TenantTests
{
    [Fact]
    public void Create_ValidInput_CreatesTenant()
    {
        // Arrange & Act
        var tenant = new Core.Entities.Tenant("Test Company", "Test Company Display");

        // Assert
        tenant.Id.Should().NotBeEmpty();
        tenant.Name.Should().Be("Test Company");
        tenant.Slug.Should().Be("test company");
        tenant.DisplayName.Should().Be("Test Company Display");
        tenant.Status.Should().Be(Shared.Contracts.Enums.TenantStatus.Active);
        tenant.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_EmptyName_ThrowsException(string name)
    {
        // Arrange & Act
        var act = () => new Core.Entities.Tenant(name, "Display");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name*");
    }

    [Fact]
    public void Suspend_ActiveTenant_SuspendsTenant()
    {
        // Arrange
        var tenant = new Core.Entities.Tenant("Name", "Display");

        // Act
        tenant.Suspend();

        // Assert
        tenant.Status.Should().Be(Shared.Contracts.Enums.TenantStatus.Suspended);
    }

    [Fact]
    public void Activate_SuspendedTenant_ActivatesTenant()
    {
        // Arrange
        var tenant = new Core.Entities.Tenant("Name", "Display");
        tenant.Suspend();

        // Act
        tenant.Activate();

        // Assert
        tenant.Status.Should().Be(Shared.Contracts.Enums.TenantStatus.Active);
    }

    [Fact]
    public void Update_ValidInput_UpdatesTenant()
    {
        // Arrange
        var tenant = new Core.Entities.Tenant("Original", "Original Display");

        // Act
        tenant.Update("Updated", "Updated Display", "https://logo.url", "#FF0000");

        // Assert
        tenant.Name.Should().Be("Updated");
        tenant.DisplayName.Should().Be("Updated Display");
        tenant.LogoUrl.Should().Be("https://logo.url");
        tenant.PrimaryColor.Should().Be("#FF0000");
        tenant.UpdatedAt.Should().NotBeNull();
    }
}
