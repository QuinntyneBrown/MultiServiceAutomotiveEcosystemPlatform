namespace Professional.Core.Interfaces;

public interface IProfessionalRepository
{
    Task<Entities.Professional?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Entities.Professional?> GetBySlugAsync(Guid tenantId, string slug, CancellationToken cancellationToken = default);
    Task<Entities.Professional?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.Professional>> GetByTenantAsync(Guid tenantId, int skip, int take, CancellationToken cancellationToken = default);
    Task<int> GetCountByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.Professional professional, CancellationToken cancellationToken = default);
    void Update(Entities.Professional professional);
    void Delete(Entities.Professional professional);
    Task<bool> SlugExistsAsync(Guid tenantId, string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(Guid tenantId, string email, Guid? excludeId = null, CancellationToken cancellationToken = default);
}

public interface ISpecialtyCatalogRepository
{
    Task<Entities.SpecialtyCatalog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.SpecialtyCatalog>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.SpecialtyCatalog>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Entities.SpecialtyCatalog specialty, CancellationToken cancellationToken = default);
    void Update(Entities.SpecialtyCatalog specialty);
}
