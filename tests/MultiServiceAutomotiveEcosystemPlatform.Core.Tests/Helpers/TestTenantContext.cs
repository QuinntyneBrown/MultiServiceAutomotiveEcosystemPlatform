using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Helpers;

public class TestTenantContext : ITenantContext
{
    public Guid TenantId { get; set; }
    public bool HasTenant => TenantId != Guid.Empty;

    public TestTenantContext(Guid? tenantId = null)
    {
        TenantId = tenantId ?? Guid.NewGuid();
    }

    public void SetTenant(Guid tenantId)
    {
        TenantId = tenantId;
    }

    public void Clear()
    {
        TenantId = Guid.Empty;
    }
}
