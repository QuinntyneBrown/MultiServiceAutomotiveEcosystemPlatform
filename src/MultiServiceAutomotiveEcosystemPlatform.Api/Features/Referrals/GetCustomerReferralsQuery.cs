// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Referrals;

public class GetCustomerReferralsQuery : IRequest<GetCustomerReferralsQueryResponse>
{
    public Guid CustomerId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 30;
}

public class GetCustomerReferralsQueryResponse
{
    public List<CustomerReferralDto> Referrals { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class GetCustomerReferralsQueryHandler : IRequestHandler<GetCustomerReferralsQuery, GetCustomerReferralsQueryResponse>
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public GetCustomerReferralsQueryHandler(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<GetCustomerReferralsQueryResponse> Handle(GetCustomerReferralsQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.TenantId;

        var query = _context.CustomerReferrals
            .Where(r => r.TenantId == tenantId && r.ReferrerCustomerId == request.CustomerId);

        var totalCount = await query.CountAsync(cancellationToken);

        var referrals = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new GetCustomerReferralsQueryResponse
        {
            Referrals = referrals.Select(r => r.ToDto()).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
