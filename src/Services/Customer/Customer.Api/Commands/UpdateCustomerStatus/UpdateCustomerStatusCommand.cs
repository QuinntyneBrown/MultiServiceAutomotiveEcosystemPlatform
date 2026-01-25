using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Enums;

namespace Customer.Api.Commands.UpdateCustomerStatus;

public record UpdateCustomerStatusCommand(Guid Id, CustomerStatus Status) : IRequest<CustomerDto>;
