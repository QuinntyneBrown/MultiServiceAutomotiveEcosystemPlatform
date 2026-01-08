// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Referrals;

public class GetProfessionalReferralsQuery : IRequest<GetProfessionalReferralsQueryResponse>
{
    public Guid ProfessionalId { get; set; }
    public string Direction { get; set; } = "Sent"; // "Sent" or "Received"
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 30;
}

public class GetProfessionalReferralsQueryResponse
{
    public List<ProfessionalReferralDto> Referrals { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class GetProfessionalReferralsQueryHandler : IRequestHandler<GetProfessionalReferralsQuery, GetProfessionalReferralsQueryResponse>
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public GetProfessionalReferralsQueryHandler(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<GetProfessionalReferralsQueryResponse> Handle(GetProfessionalReferralsQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.TenantId;

        var query = request.Direction.ToLower() == "received"
            ? _context.ProfessionalReferrals.Where(r => r.TenantId == tenantId && r.TargetProfessionalId == request.ProfessionalId)
            : _context.ProfessionalReferrals.Where(r => r.TenantId == tenantId && r.SourceProfessionalId == request.ProfessionalId);

        var totalCount = await query.CountAsync(cancellationToken);

        var referrals = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new GetProfessionalReferralsQueryResponse
        {
            Referrals = referrals.Select(r => r.ToDto()).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
