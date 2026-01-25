using Customer.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.DTOs;
using Shared.Infrastructure.Services;

namespace Customer.Api.Queries.GetOwnershipHistory;

public class GetOwnershipHistoryQueryHandler : IRequestHandler<GetOwnershipHistoryQuery, IReadOnlyList<CustomerOwnershipHistoryDto>>
{
    private readonly CustomerDbContext _context;
    private readonly ITenantContext _tenantContext;

    public GetOwnershipHistoryQueryHandler(CustomerDbContext context, ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<CustomerOwnershipHistoryDto>> Handle(GetOwnershipHistoryQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var history = await _context.CustomerOwnershipHistories
            .Where(h => h.CustomerId == request.CustomerId)
            .OrderByDescending(h => h.TransferredAt)
            .Select(h => new CustomerOwnershipHistoryDto
            {
                Id = h.Id,
                CustomerId = h.CustomerId,
                PreviousOwnerId = h.PreviousOwnerId,
                NewOwnerId = h.NewOwnerId,
                Reason = h.Reason,
                TransferredAt = h.TransferredAt
            })
            .ToListAsync(cancellationToken);

        return history;
    }
}
