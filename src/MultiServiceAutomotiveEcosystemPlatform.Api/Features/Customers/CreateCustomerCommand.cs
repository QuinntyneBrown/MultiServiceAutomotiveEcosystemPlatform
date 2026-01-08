// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Features.Customers;

public class CreateCustomerCommand : IRequest<CustomerDto>
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid? OwnerProfessionalId { get; set; }
    public string? Source { get; set; }
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public CreateCustomerCommandHandler(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.TenantId;

        // Check for duplicate email within tenant
        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.Email == request.Email.ToLowerInvariant(), cancellationToken);

        if (existingCustomer != null)
        {
            throw new InvalidOperationException($"Customer with email {request.Email} already exists.");
        }

        var customer = new Customer(
            tenantId,
            request.Email,
            request.Phone,
            request.FirstName,
            request.LastName,
            request.OwnerProfessionalId,
            request.Source);

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return customer.ToDto();
    }
}
