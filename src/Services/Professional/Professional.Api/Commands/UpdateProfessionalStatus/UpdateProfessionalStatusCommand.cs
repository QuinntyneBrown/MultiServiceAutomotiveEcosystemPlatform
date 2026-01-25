using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Enums;

namespace Professional.Api.Commands.UpdateProfessionalStatus;

public record UpdateProfessionalStatusCommand(Guid Id, ProfessionalStatus Status) : IRequest<ProfessionalDto>;
