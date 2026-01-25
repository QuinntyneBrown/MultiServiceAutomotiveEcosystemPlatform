using MediatR;
using Shared.Contracts.DTOs;

namespace Professional.Api.Commands.AddSpecialty;

public record AddSpecialtyCommand(
    Guid ProfessionalId,
    Guid SpecialtyId,
    string? Certification,
    int YearsOfExperience
) : IRequest<ProfessionalDto>;
