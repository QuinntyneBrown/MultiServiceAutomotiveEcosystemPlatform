namespace MultiServiceAutomotiveEcosystemPlatform.Core.Services;

public interface ITenantContext
{
    Guid TenantId { get; }
    bool HasTenant { get; }
}
