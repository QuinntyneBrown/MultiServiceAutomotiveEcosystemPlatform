using MediatR;
using Shared.Contracts.DTOs;

namespace Professional.Api.Commands.CreateSpecialty;

public record CreateSpecialtyCommand(
    string Name,
    string? Description,
    string? Category
) : IRequest<SpecialtyCatalogDto>;
