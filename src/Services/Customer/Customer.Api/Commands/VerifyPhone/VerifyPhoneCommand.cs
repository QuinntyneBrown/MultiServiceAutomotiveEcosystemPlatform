using MediatR;
using Shared.Contracts.DTOs;

namespace Customer.Api.Commands.VerifyPhone;

public record VerifyPhoneCommand(Guid CustomerId) : IRequest<CustomerDto>;
