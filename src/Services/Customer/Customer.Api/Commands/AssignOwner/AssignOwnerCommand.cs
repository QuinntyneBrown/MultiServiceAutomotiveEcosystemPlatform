using MediatR;
using Shared.Contracts.DTOs;

namespace Customer.Api.Commands.AssignOwner;

public record AssignOwnerCommand(
    Guid CustomerId,
    Guid ProfessionalId,
    string? Reason
) : IRequest<CustomerDto>;
