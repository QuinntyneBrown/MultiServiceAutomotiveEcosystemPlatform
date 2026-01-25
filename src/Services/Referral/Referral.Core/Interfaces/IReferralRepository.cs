namespace Referral.Core.Interfaces;

public interface ICustomerReferralRepository
{
    Task<Entities.CustomerReferral?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.CustomerReferral>> GetByReferringCustomerAsync(Guid tenantId, Guid customerId, int skip, int take, CancellationToken cancellationToken = default);
    Task<int> GetCountByReferringCustomerAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.CustomerReferral referral, CancellationToken cancellationToken = default);
    void Update(Entities.CustomerReferral referral);
}

public interface IProfessionalReferralRepository
{
    Task<Entities.ProfessionalReferral?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.ProfessionalReferral>> GetByReferringProfessionalAsync(Guid tenantId, Guid professionalId, int skip, int take, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.ProfessionalReferral>> GetByReferredProfessionalAsync(Guid tenantId, Guid professionalId, int skip, int take, CancellationToken cancellationToken = default);
    Task<int> GetCountByReferringProfessionalAsync(Guid tenantId, Guid professionalId, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.ProfessionalReferral referral, CancellationToken cancellationToken = default);
    void Update(Entities.ProfessionalReferral referral);
}

public interface IReferralCodeRepository
{
    Task<Entities.ReferralCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Entities.ReferralCode?> GetByCodeAsync(Guid tenantId, string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.ReferralCode>> GetByOwnerAsync(Guid tenantId, Guid ownerId, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.ReferralCode code, CancellationToken cancellationToken = default);
    void Update(Entities.ReferralCode code);
    Task<bool> CodeExistsAsync(Guid tenantId, string code, CancellationToken cancellationToken = default);
}

public interface IReferralStatsRepository
{
    Task<Entities.ReferralStats?> GetByOwnerAsync(Guid tenantId, Guid ownerId, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.ReferralStats stats, CancellationToken cancellationToken = default);
    void Update(Entities.ReferralStats stats);
}
