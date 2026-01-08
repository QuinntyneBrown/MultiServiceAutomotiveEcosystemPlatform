using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Services;

public interface IProfessionalService
{
    Task<Professional> CreateProfessionalAsync(
        Guid userId,
        string businessName,
        BusinessType businessType,
        string firstName,
        string lastName,
        string email,
        string phone,
        string addressLine1,
        string city,
        string province,
        string postalCode,
        CancellationToken cancellationToken = default);

    Task<Professional?> GetProfessionalByIdAsync(Guid professionalId, CancellationToken cancellationToken = default);
    Task<Professional?> GetProfessionalBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Professional?> GetProfessionalByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Professional>> GetActiveProfessionalsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Professional>> GetProfessionalsByTypeAsync(BusinessType businessType, CancellationToken cancellationToken = default);
    Task<IEnumerable<Professional>> GetFeaturedProfessionalsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Professional>> GetProfessionalsAcceptingReferralsAsync(CancellationToken cancellationToken = default);

    Task<Professional> UpdateProfessionalAsync(
        Guid professionalId,
        string businessName,
        BusinessType businessType,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default);

    Task ActivateProfessionalAsync(Guid professionalId, CancellationToken cancellationToken = default);
    Task SuspendProfessionalAsync(Guid professionalId, CancellationToken cancellationToken = default);
    Task VerifyProfessionalAsync(Guid professionalId, CancellationToken cancellationToken = default);
    Task SetFeaturedAsync(Guid professionalId, bool featured, CancellationToken cancellationToken = default);

    Task<ProfessionalSpecialty> AddSpecialtyAsync(
        Guid professionalId,
        Guid? specialtyId,
        string? customName,
        string? description,
        int? yearsExperience,
        CancellationToken cancellationToken = default);

    Task RemoveSpecialtyAsync(Guid professionalId, Guid specialtyId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SpecialtyCatalog>> GetSpecialtyCatalogAsync(CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeProfessionalId = null, CancellationToken cancellationToken = default);
}
