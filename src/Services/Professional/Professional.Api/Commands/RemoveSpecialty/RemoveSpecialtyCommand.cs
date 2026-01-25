using MediatR;

namespace Professional.Api.Commands.RemoveSpecialty;

public record RemoveSpecialtyCommand(Guid ProfessionalId, Guid SpecialtyId) : IRequest<bool>;
