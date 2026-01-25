using MediatR;
using Shared.Contracts.DTOs;

namespace Professional.Api.Commands.VerifyProfessional;

public record VerifyProfessionalCommand(Guid Id) : IRequest<ProfessionalDto>;
