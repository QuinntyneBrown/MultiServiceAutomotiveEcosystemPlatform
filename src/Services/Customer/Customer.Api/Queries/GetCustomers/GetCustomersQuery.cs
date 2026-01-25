using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;

namespace Customer.Api.Queries.GetCustomers;

public record GetCustomersQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<CustomerDto>>;
