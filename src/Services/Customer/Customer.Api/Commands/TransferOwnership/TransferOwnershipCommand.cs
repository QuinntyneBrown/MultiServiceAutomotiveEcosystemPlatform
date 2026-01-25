using MediatR;
using Shared.Contracts.DTOs;

namespace Customer.Api.Commands.TransferOwnership;

public record TransferOwnershipCommand(
    Guid CustomerId,
    Guid NewOwnerId,
    string Reason
) : IRequest<CustomerDto>;
