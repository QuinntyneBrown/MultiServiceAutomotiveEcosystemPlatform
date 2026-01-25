using MediatR;
using Professional.Core.Interfaces;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;
using Shared.Infrastructure.Services;

namespace Professional.Api.Queries.GetProfessionals;

public class GetProfessionalsQueryHandler : IRequestHandler<GetProfessionalsQuery, PagedResult<ProfessionalDto>>
{
    private readonly IProfessionalRepository _repository;
    private readonly ITenantContext _tenantContext;

    public GetProfessionalsQueryHandler(IProfessionalRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<PagedResult<ProfessionalDto>> Handle(GetProfessionalsQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var skip = (page - 1) * pageSize;

        var professionals = await _repository.GetByTenantAsync(_tenantContext.TenantId, skip, pageSize, cancellationToken);
        var totalCount = await _repository.GetCountByTenantAsync(_tenantContext.TenantId, cancellationToken);

        var items = professionals.Select(MapToDto).ToList();

        return PagedResult<ProfessionalDto>.Create(items, totalCount, page, pageSize);
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
