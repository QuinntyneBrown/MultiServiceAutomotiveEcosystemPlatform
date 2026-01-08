// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Referrals;

public class CreateProfessionalReferralCommand : IRequest<ProfessionalReferralDto>
{
    public Guid SourceProfessionalId { get; set; }
    public Guid TargetProfessionalId { get; set; }
    public Guid CustomerId { get; set; }
    public string? Reason { get; set; }
    public string? ServiceNeeded { get; set; }
    public string? Notes { get; set; }
    public string Priority { get; set; } = "Normal";
    public bool OfferDiscount { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
}

public class CreateProfessionalReferralCommandHandler : IRequestHandler<CreateProfessionalReferralCommand, ProfessionalReferralDto>
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IReferralCodeGenerator _referralCodeGenerator;

    public CreateProfessionalReferralCommandHandler(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext,
        IReferralCodeGenerator referralCodeGenerator)
    {
        _context = context;
        _tenantContext = tenantContext;
        _referralCodeGenerator = referralCodeGenerator;
    }

    public async Task<ProfessionalReferralDto> Handle(CreateProfessionalReferralCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.TenantId;

        // Verify all parties exist
        var sourceProfessional = await _context.Professionals
            .FirstOrDefaultAsync(p => p.TenantId == tenantId && p.ProfessionalId == request.SourceProfessionalId, cancellationToken);
        var targetProfessional = await _context.Professionals
            .FirstOrDefaultAsync(p => p.TenantId == tenantId && p.ProfessionalId == request.TargetProfessionalId, cancellationToken);
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.CustomerId == request.CustomerId, cancellationToken);

        if (sourceProfessional == null || targetProfessional == null || customer == null)
        {
            throw new InvalidOperationException("One or more parties in the referral do not exist.");
        }

        if (!Enum.TryParse<ReferralPriority>(request.Priority, out var priority))
        {
            priority = ReferralPriority.Normal;
        }

        // Create professional referral
        var referral = new ProfessionalReferral(
            tenantId,
            request.SourceProfessionalId,
            request.TargetProfessionalId,
            request.CustomerId,
            request.Reason,
            request.ServiceNeeded,
            request.Notes,
            priority);

        // Set discount if offered
        if (request.OfferDiscount && !string.IsNullOrWhiteSpace(request.DiscountType))
        {
            if (!Enum.TryParse<DiscountType>(request.DiscountType, out var discountType))
            {
                discountType = DiscountType.None;
            }

            if (discountType != DiscountType.None)
            {
                var discountCode = _referralCodeGenerator.GenerateDiscountCode();
                referral.SetDiscount(discountType, request.DiscountValue, discountCode);
            }
        }

        _context.ProfessionalReferrals.Add(referral);
        await _context.SaveChangesAsync(cancellationToken);

        return referral.ToDto();
    }
}
