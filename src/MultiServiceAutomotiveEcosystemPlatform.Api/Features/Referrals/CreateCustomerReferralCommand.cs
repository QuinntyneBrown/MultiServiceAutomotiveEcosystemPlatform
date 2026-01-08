// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Referrals;

public class CreateCustomerReferralCommand : IRequest<CustomerReferralDto>
{
    public Guid ReferrerCustomerId { get; set; }
    public string? RefereeEmail { get; set; }
    public string? RefereePhone { get; set; }
    public string? RefereeName { get; set; }
    public Guid? TargetProfessionalId { get; set; }
    public string? TargetServiceType { get; set; }
    public string? ReferralSource { get; set; }
}

public class CreateCustomerReferralCommandHandler : IRequestHandler<CreateCustomerReferralCommand, CustomerReferralDto>
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IReferralCodeGenerator _referralCodeGenerator;

    public CreateCustomerReferralCommandHandler(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext,
        IReferralCodeGenerator referralCodeGenerator)
    {
        _context = context;
        _tenantContext = tenantContext;
        _referralCodeGenerator = referralCodeGenerator;
    }

    public async Task<CustomerReferralDto> Handle(CreateCustomerReferralCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.TenantId;

        // Verify referrer customer exists
        var referrer = await _context.Customers
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.CustomerId == request.ReferrerCustomerId, cancellationToken);

        if (referrer == null)
        {
            throw new InvalidOperationException("Referrer customer not found.");
        }

        // Get or create referral code for customer
        var referralCode = await _context.ReferralCodes
            .FirstOrDefaultAsync(rc => rc.TenantId == tenantId && rc.CustomerId == request.ReferrerCustomerId, cancellationToken);

        if (referralCode == null)
        {
            var code = _referralCodeGenerator.GenerateCode(referrer.FirstName);
            
            // Check if code already exists, if so generate a new one
            while (await _context.ReferralCodes.AnyAsync(rc => rc.TenantId == tenantId && rc.Code == code, cancellationToken))
            {
                code = _referralCodeGenerator.GenerateCode(referrer.FirstName);
            }
            
            referralCode = new ReferralCode(
                tenantId,
                code,
                Core.Models.ReferralAggregate.Enums.ReferralCodeType.Customer,
                customerId: request.ReferrerCustomerId);

            _context.ReferralCodes.Add(referralCode);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Create referral
        var referral = new CustomerReferral(
            tenantId,
            request.ReferrerCustomerId,
            referralCode.Code,
            request.RefereeEmail,
            request.RefereePhone,
            request.RefereeName,
            request.TargetProfessionalId,
            request.TargetServiceType,
            request.ReferralSource);

        _context.CustomerReferrals.Add(referral);
        await _context.SaveChangesAsync(cancellationToken);

        return referral.ToDto();
    }
}
