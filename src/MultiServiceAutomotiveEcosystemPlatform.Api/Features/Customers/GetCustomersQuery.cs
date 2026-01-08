// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Customers;

public class GetCustomersQuery : IRequest<GetCustomersQueryResponse>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 30;
}

public class GetCustomersQueryResponse
{
    public List<CustomerDto> Customers { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, GetCustomersQueryResponse>
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public GetCustomersQueryHandler(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<GetCustomersQueryResponse> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.TenantId;

        var query = _context.Customers
            .Where(c => c.TenantId == tenantId);

        var totalCount = await query.CountAsync(cancellationToken);

        var customers = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new GetCustomersQueryResponse
        {
            Customers = customers.Select(c => c.ToDto()).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
