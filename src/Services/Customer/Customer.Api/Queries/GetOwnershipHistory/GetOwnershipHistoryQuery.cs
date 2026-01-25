using MediatR;
using Shared.Contracts.DTOs;

namespace Customer.Api.Queries.GetOwnershipHistory;

public record GetOwnershipHistoryQuery(Guid CustomerId) : IRequest<IReadOnlyList<CustomerOwnershipHistoryDto>>;
