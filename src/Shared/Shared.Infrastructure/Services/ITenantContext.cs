namespace Shared.Infrastructure.Services;

public interface ITenantContext
{
    Guid TenantId { get; }
    bool HasTenant { get; }
    void SetTenant(Guid tenantId);
}
