// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Customers;

public class GetCustomerQuery : IRequest<CustomerDto?>
{
    public Guid CustomerId { get; set; }
}

public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDto?>
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public GetCustomerQueryHandler(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<CustomerDto?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.TenantId;

        var customer = await _context.Customers
            .FirstOrDefaultAsync(
                c => c.TenantId == tenantId && c.CustomerId == request.CustomerId,
                cancellationToken);

        return customer?.ToDto();
    }
}
