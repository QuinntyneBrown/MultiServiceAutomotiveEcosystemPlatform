using MassTransit;
using MediatR;
using Professional.Core.Interfaces;
using Professional.Infrastructure.Data;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Professional;
using Shared.Infrastructure.Services;

namespace Professional.Api.Commands.CreateProfessional;

public class CreateProfessionalCommandHandler : IRequestHandler<CreateProfessionalCommand, ProfessionalDto>
{
    private readonly IProfessionalRepository _repository;
    private readonly ProfessionalDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CreateProfessionalCommandHandler> _logger;

    public CreateProfessionalCommandHandler(
        IProfessionalRepository repository,
        ProfessionalDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<CreateProfessionalCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ProfessionalDto> Handle(CreateProfessionalCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        // Check for duplicate email
        if (await _repository.EmailExistsAsync(_tenantContext.TenantId, request.Email, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException($"A professional with email '{request.Email}' already exists");
        }

        var professional = new Core.Entities.Professional(
            _tenantContext.TenantId,
            request.BusinessName,
            request.FirstName,
            request.LastName,
            request.Email);

        professional.UpdateBusinessInfo(
            request.BusinessName,
            request.BusinessDescription,
            request.BusinessPhone,
            request.BusinessEmail,
            request.Website);

        professional.UpdatePersonalInfo(request.FirstName, request.LastName, request.Email, request.Phone);
        professional.UpdateAddress(request.StreetAddress, request.City, request.State, request.PostalCode, request.Country);
        professional.UpdateLocation(request.Latitude, request.Longitude);
        professional.UpdateReferralSettings(request.AcceptsReferrals, request.ReferralCommissionRate);

        await _repository.AddAsync(professional, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new ProfessionalCreated(
            professional.Id,
            professional.TenantId,
            professional.BusinessName,
            professional.Email,
            professional.Slug,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Created professional {ProfessionalId} for tenant {TenantId}", professional.Id, professional.TenantId);

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
