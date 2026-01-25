namespace Shared.Infrastructure.Services;

public class TenantContext : ITenantContext
{
    private Guid _tenantId;

    public Guid TenantId => _tenantId;

    public bool HasTenant => _tenantId != Guid.Empty;

    public void SetTenant(Guid tenantId)
    {
        _tenantId = tenantId;
    }
}
