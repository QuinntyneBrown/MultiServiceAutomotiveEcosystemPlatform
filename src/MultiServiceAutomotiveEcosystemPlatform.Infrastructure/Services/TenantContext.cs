using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Services;

public class TenantContext : ITenantContext
{
    private Guid? _tenantId;

    public Guid TenantId
    {
        get => _tenantId ?? throw new InvalidOperationException("Tenant context has not been set.");
        private set => _tenantId = value;
    }

    public bool HasTenant => _tenantId.HasValue;

    public void SetTenant(Guid tenantId)
    {
        if (_tenantId.HasValue)
            throw new InvalidOperationException("Tenant context has already been set for this request.");

        _tenantId = tenantId;
    }

    public void Clear()
    {
        _tenantId = null;
    }
}
