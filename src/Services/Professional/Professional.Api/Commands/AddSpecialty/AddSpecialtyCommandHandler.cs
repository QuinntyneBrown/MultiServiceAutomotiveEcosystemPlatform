using MassTransit;
using MediatR;
using Professional.Core.Interfaces;
using Professional.Infrastructure.Data;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Professional;
using Shared.Infrastructure.Services;

namespace Professional.Api.Commands.AddSpecialty;

public class AddSpecialtyCommandHandler : IRequestHandler<AddSpecialtyCommand, ProfessionalDto>
{
    private readonly IProfessionalRepository _repository;
    private readonly ISpecialtyCatalogRepository _specialtyRepository;
    private readonly ProfessionalDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<AddSpecialtyCommandHandler> _logger;

    public AddSpecialtyCommandHandler(
        IProfessionalRepository repository,
        ISpecialtyCatalogRepository specialtyRepository,
        ProfessionalDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<AddSpecialtyCommandHandler> logger)
    {
        _repository = repository;
        _specialtyRepository = specialtyRepository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ProfessionalDto> Handle(AddSpecialtyCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var professional = await _repository.GetByIdAsync(request.ProfessionalId, cancellationToken)
            ?? throw new KeyNotFoundException($"Professional with ID {request.ProfessionalId} not found");

        var specialty = await _specialtyRepository.GetByIdAsync(request.SpecialtyId, cancellationToken)
            ?? throw new KeyNotFoundException($"Specialty with ID {request.SpecialtyId} not found");

        professional.AddSpecialty(specialty.Id, specialty.Name, request.Certification, request.YearsOfExperience);

        _repository.Update(professional);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new ProfessionalSpecialtyAdded(
            professional.Id,
            professional.TenantId,
            specialty.Id,
            specialty.Name,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Added specialty {SpecialtyId} to professional {ProfessionalId}",
            specialty.Id, professional.Id);

        return MapToDto(professional);
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
