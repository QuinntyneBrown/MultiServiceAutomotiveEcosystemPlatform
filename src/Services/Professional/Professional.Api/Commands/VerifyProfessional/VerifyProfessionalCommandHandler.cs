using MassTransit;
using MediatR;
using Professional.Core.Interfaces;
using Professional.Infrastructure.Data;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Professional;
using Shared.Infrastructure.Services;

namespace Professional.Api.Commands.VerifyProfessional;

public class VerifyProfessionalCommandHandler : IRequestHandler<VerifyProfessionalCommand, ProfessionalDto>
{
    private readonly IProfessionalRepository _repository;
    private readonly ProfessionalDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<VerifyProfessionalCommandHandler> _logger;

    public VerifyProfessionalCommandHandler(
        IProfessionalRepository repository,
        ProfessionalDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<VerifyProfessionalCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ProfessionalDto> Handle(VerifyProfessionalCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var professional = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Professional with ID {request.Id} not found");

        professional.Verify();

        _repository.Update(professional);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new ProfessionalVerified(
            professional.Id,
            professional.TenantId,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Verified professional {ProfessionalId}", professional.Id);

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
