using MediatR;
using Professional.Core.Interfaces;
using Shared.Contracts.DTOs;
using Shared.Infrastructure.Services;

namespace Professional.Api.Queries.GetProfessional;

public class GetProfessionalQueryHandler : IRequestHandler<GetProfessionalQuery, ProfessionalDto?>
{
    private readonly IProfessionalRepository _repository;

    public GetProfessionalQueryHandler(IProfessionalRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProfessionalDto?> Handle(GetProfessionalQuery request, CancellationToken cancellationToken)
    {
        var professional = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return professional is null ? null : MapToDto(professional);
    }

    private static ProfessionalDto MapToDto(Core.Entities.Professional professional) => new()
    {
        Id = professional.Id,
        TenantId = professional.TenantId,
        BusinessName = professional.BusinessName,
        BusinessDescription = professional.BusinessDescription,
        BusinessPhone = professional.BusinessPhone,
        BusinessEmail = professional.BusinessEmail,
        Website = professional.Website,
        Slug = professional.Slug,
        FirstName = professional.FirstName,
        LastName = professional.LastName,
        Email = professional.Email,
        Phone = professional.Phone,
        StreetAddress = professional.StreetAddress,
        City = professional.City,
        State = professional.State,
        PostalCode = professional.PostalCode,
        Country = professional.Country,
        Latitude = professional.Latitude,
        Longitude = professional.Longitude,
        ProfileImageUrl = professional.ProfileImageUrl,
        CoverImageUrl = professional.CoverImageUrl,
        Status = professional.Status,
        IsVerified = professional.IsVerified,
        IsFeatured = professional.IsFeatured,
        AcceptsReferrals = professional.AcceptsReferrals,
        ReferralCommissionRate = professional.ReferralCommissionRate,
        Specialties = professional.Specialties.Select(s => new ProfessionalSpecialtyDto
        {
            Id = s.Id,
            ProfessionalId = s.ProfessionalId,
            SpecialtyId = s.SpecialtyId,
            SpecialtyName = s.SpecialtyName,
            Certification = s.Certification,
            IsVerified = s.IsVerified,
            YearsOfExperience = s.YearsOfExperience
        }).ToList(),
        CreatedAt = professional.CreatedAt,
        UpdatedAt = professional.UpdatedAt
    };
}

public class GetProfessionalBySlugQueryHandler : IRequestHandler<GetProfessionalBySlugQuery, ProfessionalDto?>
{
    private readonly IProfessionalRepository _repository;
    private readonly ITenantContext _tenantContext;

    public GetProfessionalBySlugQueryHandler(IProfessionalRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<ProfessionalDto?> Handle(GetProfessionalBySlugQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var professional = await _repository.GetBySlugAsync(_tenantContext.TenantId, request.Slug, cancellationToken);
        return professional is null ? null : MapToDto(professional);
    }

    private static ProfessionalDto MapToDto(Core.Entities.Professional professional) => new()
    {
        Id = professional.Id,
        TenantId = professional.TenantId,
        BusinessName = professional.BusinessName,
        BusinessDescription = professional.BusinessDescription,
        BusinessPhone = professional.BusinessPhone,
        BusinessEmail = professional.BusinessEmail,
        Website = professional.Website,
        Slug = professional.Slug,
        FirstName = professional.FirstName,
        LastName = professional.LastName,
        Email = professional.Email,
        Phone = professional.Phone,
        StreetAddress = professional.StreetAddress,
        City = professional.City,
        State = professional.State,
        PostalCode = professional.PostalCode,
        Country = professional.Country,
        Latitude = professional.Latitude,
        Longitude = professional.Longitude,
        ProfileImageUrl = professional.ProfileImageUrl,
        CoverImageUrl = professional.CoverImageUrl,
        Status = professional.Status,
        IsVerified = professional.IsVerified,
        IsFeatured = professional.IsFeatured,
        AcceptsReferrals = professional.AcceptsReferrals,
        ReferralCommissionRate = professional.ReferralCommissionRate,
        Specialties = professional.Specialties.Select(s => new ProfessionalSpecialtyDto
        {
            Id = s.Id,
            ProfessionalId = s.ProfessionalId,
            SpecialtyId = s.SpecialtyId,
            SpecialtyName = s.SpecialtyName,
            Certification = s.Certification,
            IsVerified = s.IsVerified,
            YearsOfExperience = s.YearsOfExperience
        }).ToList(),
        CreatedAt = professional.CreatedAt,
        UpdatedAt = professional.UpdatedAt
    };
}
