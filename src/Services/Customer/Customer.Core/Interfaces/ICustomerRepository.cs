namespace Customer.Core.Interfaces;

public interface ICustomerRepository
{
    Task<Entities.Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Entities.Customer?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.Customer>> GetByTenantAsync(Guid tenantId, int skip, int take, CancellationToken cancellationToken = default);
    Task<int> GetCountByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.Customer>> GetByOwnerAsync(Guid tenantId, Guid ownerId, int skip, int take, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.Customer customer, CancellationToken cancellationToken = default);
    void Update(Entities.Customer customer);
    void Delete(Entities.Customer customer);
    Task<bool> EmailExistsAsync(Guid tenantId, string email, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
