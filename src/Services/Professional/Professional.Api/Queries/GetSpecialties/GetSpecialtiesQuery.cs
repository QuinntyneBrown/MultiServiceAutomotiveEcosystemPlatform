using MediatR;
using Shared.Contracts.DTOs;

namespace Professional.Api.Queries.GetSpecialties;

public record GetSpecialtiesQuery : IRequest<IReadOnlyList<SpecialtyCatalogDto>>;
