using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Services;

public interface ICustomerService
{
    Task<Customer> CreateCustomerAsync(
        string email,
        string phone,
        string firstName,
        string lastName,
        Guid? ownerProfessionalId = null,
        string? source = null,
        CancellationToken cancellationToken = default);

    Task<Customer?> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetCustomersByOwnerAsync(Guid ownerProfessionalId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);

    Task<Customer> UpdateCustomerAsync(
        Guid customerId,
        string email,
        string phone,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default);

    Task TransferOwnershipAsync(
        Guid customerId,
        Guid newOwnerId,
        string reason,
        Guid transferredBy,
        CancellationToken cancellationToken = default);

    Task<bool> AssignOwnerIfNotSetAsync(Guid customerId, Guid professionalId, CancellationToken cancellationToken = default);
    Task DeactivateCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, Guid? excludeCustomerId = null, CancellationToken cancellationToken = default);
}
