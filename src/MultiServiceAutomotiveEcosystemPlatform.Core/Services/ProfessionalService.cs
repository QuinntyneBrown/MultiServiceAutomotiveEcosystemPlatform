using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Services;

public class ProfessionalService : IProfessionalService
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public ProfessionalService(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<Professional> CreateProfessionalAsync(
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
        CancellationToken cancellationToken = default)
    {
        var professional = new Professional(
            _tenantContext.TenantId,
            userId,
            businessName,
            businessType,
            firstName,
            lastName,
            email,
            phone,
            addressLine1,
            city,
            province,
            postalCode);

        // Ensure slug is unique
        var baseSlug = professional.Slug;
        var slug = baseSlug;
        var counter = 1;

        while (await SlugExistsAsync(slug, cancellationToken: cancellationToken))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        // If slug changed, we need to handle this through a method or the entity
        // For now, we'll accept the generated slug

        _context.Professionals.Add(professional);
        await _context.SaveChangesAsync(cancellationToken);

        return professional;
    }

    public async Task<Professional?> GetProfessionalByIdAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Include(p => p.Specialties)
            .FirstOrDefaultAsync(p => p.ProfessionalId == professionalId && p.TenantId == _tenantContext.TenantId, cancellationToken);
    }

    public async Task<Professional?> GetProfessionalBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.ToLowerInvariant().Trim();
        return await _context.Professionals
            .Include(p => p.Specialties)
            .FirstOrDefaultAsync(p => p.Slug == normalizedSlug && p.TenantId == _tenantContext.TenantId, cancellationToken);
    }

    public async Task<Professional?> GetProfessionalByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Include(p => p.Specialties)
            .FirstOrDefaultAsync(p => p.UserId == userId && p.TenantId == _tenantContext.TenantId, cancellationToken);
    }

    public async Task<IEnumerable<Professional>> GetActiveProfessionalsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Include(p => p.Specialties)
            .Where(p => p.Status == ProfessionalStatus.Active && p.TenantId == _tenantContext.TenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Professional>> GetProfessionalsByTypeAsync(BusinessType businessType, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Include(p => p.Specialties)
            .Where(p => p.BusinessType == businessType && p.Status == ProfessionalStatus.Active && p.TenantId == _tenantContext.TenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Professional>> GetFeaturedProfessionalsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Include(p => p.Specialties)
            .Where(p => p.Featured && p.Status == ProfessionalStatus.Active && p.TenantId == _tenantContext.TenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Professional>> GetProfessionalsAcceptingReferralsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Include(p => p.Specialties)
            .Where(p => p.AcceptsReferrals && p.Status == ProfessionalStatus.Active && p.TenantId == _tenantContext.TenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Professional> UpdateProfessionalAsync(
        Guid professionalId,
        string businessName,
        BusinessType businessType,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default)
    {
        var professional = await GetProfessionalByIdAsync(professionalId, cancellationToken)
            ?? throw new InvalidOperationException($"Professional with ID '{professionalId}' not found.");

        professional.UpdateBusinessInfo(businessName, businessType, professional.LicenseNumber, professional.TaxId);
        professional.UpdatePersonalInfo(firstName, lastName, professional.Title, professional.Bio);

        await _context.SaveChangesAsync(cancellationToken);

        return professional;
    }

    public async Task ActivateProfessionalAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        var professional = await GetProfessionalByIdAsync(professionalId, cancellationToken)
            ?? throw new InvalidOperationException($"Professional with ID '{professionalId}' not found.");

        professional.Activate();
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SuspendProfessionalAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        var professional = await GetProfessionalByIdAsync(professionalId, cancellationToken)
            ?? throw new InvalidOperationException($"Professional with ID '{professionalId}' not found.");

        professional.Suspend();
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task VerifyProfessionalAsync(Guid professionalId, CancellationToken cancellationToken = default)
    {
        var professional = await GetProfessionalByIdAsync(professionalId, cancellationToken)
            ?? throw new InvalidOperationException($"Professional with ID '{professionalId}' not found.");

        professional.Verify();
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetFeaturedAsync(Guid professionalId, bool featured, CancellationToken cancellationToken = default)
    {
        var professional = await GetProfessionalByIdAsync(professionalId, cancellationToken)
            ?? throw new InvalidOperationException($"Professional with ID '{professionalId}' not found.");

        professional.SetFeatured(featured);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ProfessionalSpecialty> AddSpecialtyAsync(
        Guid professionalId,
        Guid? specialtyId,
        string? customName,
        string? description,
        int? yearsExperience,
        CancellationToken cancellationToken = default)
    {
        var professional = await GetProfessionalByIdAsync(professionalId, cancellationToken)
            ?? throw new InvalidOperationException($"Professional with ID '{professionalId}' not found.");

        var displayOrder = professional.Specialties.Count;
        var specialty = new ProfessionalSpecialty(professionalId, specialtyId, customName, description, yearsExperience, displayOrder);

        professional.AddSpecialty(specialty);
        await _context.SaveChangesAsync(cancellationToken);

        return specialty;
    }

    public async Task RemoveSpecialtyAsync(Guid professionalId, Guid specialtyId, CancellationToken cancellationToken = default)
    {
        var professional = await GetProfessionalByIdAsync(professionalId, cancellationToken)
            ?? throw new InvalidOperationException($"Professional with ID '{professionalId}' not found.");

        var specialty = professional.Specialties.FirstOrDefault(s => s.ProfessionalSpecialtyId == specialtyId);
        if (specialty != null)
        {
            professional.RemoveSpecialty(specialty);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<SpecialtyCatalog>> GetSpecialtyCatalogAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SpecialtyCatalogs
            .Where(s => s.IsActive && (s.TenantId == null || s.TenantId == _tenantContext.TenantId))
            .OrderBy(s => s.Category)
            .ThenBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeProfessionalId = null, CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.ToLowerInvariant().Trim();

        var query = _context.Professionals
            .Where(p => p.Slug == normalizedSlug && p.TenantId == _tenantContext.TenantId);

        if (excludeProfessionalId.HasValue)
            query = query.Where(p => p.ProfessionalId != excludeProfessionalId.Value);

        return await query.AnyAsync(cancellationToken);
    }
}
