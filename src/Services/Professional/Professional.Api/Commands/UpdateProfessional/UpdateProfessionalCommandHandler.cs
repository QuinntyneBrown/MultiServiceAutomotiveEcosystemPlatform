using MassTransit;
using MediatR;
using Professional.Core.Interfaces;
using Professional.Infrastructure.Data;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Professional;
using Shared.Infrastructure.Services;

namespace Professional.Api.Commands.UpdateProfessional;

public class UpdateProfessionalCommandHandler : IRequestHandler<UpdateProfessionalCommand, ProfessionalDto>
{
    private readonly IProfessionalRepository _repository;
    private readonly ProfessionalDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<UpdateProfessionalCommandHandler> _logger;

    public UpdateProfessionalCommandHandler(
        IProfessionalRepository repository,
        ProfessionalDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<UpdateProfessionalCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ProfessionalDto> Handle(UpdateProfessionalCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var professional = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Professional with ID {request.Id} not found");

        // Check for duplicate email (excluding current professional)
        if (await _repository.EmailExistsAsync(_tenantContext.TenantId, request.Email, request.Id, cancellationToken))
        {
            throw new InvalidOperationException($"A professional with email '{request.Email}' already exists");
        }

        professional.UpdateBusinessInfo(request.BusinessName, request.BusinessDescription, request.BusinessPhone, request.BusinessEmail, request.Website);
        professional.UpdatePersonalInfo(request.FirstName, request.LastName, request.Email, request.Phone);
        professional.UpdateAddress(request.StreetAddress, request.City, request.State, request.PostalCode, request.Country);
        professional.UpdateLocation(request.Latitude, request.Longitude);
        professional.UpdateMedia(request.ProfileImageUrl, request.CoverImageUrl);
        professional.UpdateReferralSettings(request.AcceptsReferrals, request.ReferralCommissionRate);

        _repository.Update(professional);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new ProfessionalUpdated(
            professional.Id,
            professional.TenantId,
            professional.BusinessName,
            professional.Email,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Updated professional {ProfessionalId} for tenant {TenantId}", professional.Id, professional.TenantId);

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
