namespace Tenant.Core.Interfaces;

public interface ITenantRepository
{
    Task<Entities.Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Entities.Tenant?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.Tenant>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Entities.Tenant tenant, CancellationToken cancellationToken = default);
    void Update(Entities.Tenant tenant);
    void Delete(Entities.Tenant tenant);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
