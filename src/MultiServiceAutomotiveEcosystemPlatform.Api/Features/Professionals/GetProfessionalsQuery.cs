// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Professionals;

public class GetProfessionalsQuery : IRequest<GetProfessionalsQueryResponse>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 30;
}

public class GetProfessionalsQueryResponse
{
    public List<ProfessionalDto> Professionals { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class GetProfessionalsQueryHandler : IRequestHandler<GetProfessionalsQuery, GetProfessionalsQueryResponse>
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public GetProfessionalsQueryHandler(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<GetProfessionalsQueryResponse> Handle(GetProfessionalsQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.TenantId;

        var query = _context.Professionals
            .Where(p => p.TenantId == tenantId);

        var totalCount = await query.CountAsync(cancellationToken);

        var professionals = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new GetProfessionalsQueryResponse
        {
            Professionals = professionals.Select(p => p.ToDto()).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
