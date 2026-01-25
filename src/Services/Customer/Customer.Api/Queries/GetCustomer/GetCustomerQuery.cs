using MediatR;
using Shared.Contracts.DTOs;

namespace Customer.Api.Queries.GetCustomer;

public record GetCustomerQuery(Guid Id) : IRequest<CustomerDto?>;
