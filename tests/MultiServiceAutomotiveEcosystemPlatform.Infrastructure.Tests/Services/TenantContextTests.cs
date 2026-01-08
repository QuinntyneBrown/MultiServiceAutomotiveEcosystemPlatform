using MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Tests.Services;

public class TenantContextTests
{
    [Fact]
    public void SetTenant_SetsTenantId()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId = Guid.NewGuid();

        // Act
        tenantContext.SetTenant(tenantId);

        // Assert
        Assert.Equal(tenantId, tenantContext.TenantId);
        Assert.True(tenantContext.HasTenant);
    }

    [Fact]
    public void SetTenant_ThrowsException_WhenAlreadySet()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();
        tenantContext.SetTenant(tenantId1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tenantContext.SetTenant(tenantId2));
    }

    [Fact]
    public void TenantId_ThrowsException_WhenNotSet()
    {
        // Arrange
        var tenantContext = new TenantContext();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tenantContext.TenantId);
    }

    [Fact]
    public void HasTenant_ReturnsFalse_WhenNotSet()
    {
        // Arrange
        var tenantContext = new TenantContext();

        // Act & Assert
        Assert.False(tenantContext.HasTenant);
    }

    [Fact]
    public void Clear_ClearsTenantId()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId = Guid.NewGuid();
        tenantContext.SetTenant(tenantId);

        // Act
        tenantContext.Clear();

        // Assert
        Assert.False(tenantContext.HasTenant);
        Assert.Throws<InvalidOperationException>(() => tenantContext.TenantId);
    }

    [Fact]
    public void CanSetTenantAgain_AfterClear()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();
        tenantContext.SetTenant(tenantId1);
        tenantContext.Clear();

        // Act
        tenantContext.SetTenant(tenantId2);

        // Assert
        Assert.Equal(tenantId2, tenantContext.TenantId);
        Assert.True(tenantContext.HasTenant);
    }
}
