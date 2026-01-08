// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate.Enums;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Professionals;

public class CreateProfessionalCommand : IRequest<ProfessionalDto>
{
    public Guid UserId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

public class CreateProfessionalCommandHandler : IRequestHandler<CreateProfessionalCommand, ProfessionalDto>
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public CreateProfessionalCommandHandler(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<ProfessionalDto> Handle(CreateProfessionalCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.TenantId;

        // Check for duplicate email within tenant
        var existingProfessional = await _context.Professionals
            .FirstOrDefaultAsync(p => p.TenantId == tenantId && p.Email == request.Email.ToLowerInvariant(), cancellationToken);

        if (existingProfessional != null)
        {
            throw new InvalidOperationException($"Professional with email {request.Email} already exists.");
        }

        if (!Enum.TryParse<BusinessType>(request.BusinessType, out var businessType))
        {
            throw new ArgumentException($"Invalid business type: {request.BusinessType}");
        }

        var professional = new Professional(
            tenantId,
            request.UserId,
            request.BusinessName,
            businessType,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.AddressLine1,
            request.City,
            request.State,
            request.PostalCode);

        if (!string.IsNullOrWhiteSpace(request.AddressLine2))
        {
            professional.UpdateAddress(
                request.AddressLine1,
                request.AddressLine2,
                request.City,
                request.State,
                request.PostalCode);
        }

        _context.Professionals.Add(professional);
        await _context.SaveChangesAsync(cancellationToken);

        return professional.ToDto();
    }
}
