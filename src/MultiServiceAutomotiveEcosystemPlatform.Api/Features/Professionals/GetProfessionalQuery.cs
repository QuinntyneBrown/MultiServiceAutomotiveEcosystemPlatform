// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Professionals;

public class GetProfessionalQuery : IRequest<ProfessionalDto?>
{
    public Guid ProfessionalId { get; set; }
}

public class GetProfessionalQueryHandler : IRequestHandler<GetProfessionalQuery, ProfessionalDto?>
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public GetProfessionalQueryHandler(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<ProfessionalDto?> Handle(GetProfessionalQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.TenantId;

        var professional = await _context.Professionals
            .FirstOrDefaultAsync(p => p.TenantId == tenantId && p.ProfessionalId == request.ProfessionalId, cancellationToken);

        return professional?.ToDto();
    }
}
