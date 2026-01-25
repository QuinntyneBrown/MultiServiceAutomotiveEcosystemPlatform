using MediatR;
using Shared.Contracts.DTOs;

namespace Customer.Api.Commands.VerifyEmail;

public record VerifyEmailCommand(Guid CustomerId) : IRequest<CustomerDto>;
