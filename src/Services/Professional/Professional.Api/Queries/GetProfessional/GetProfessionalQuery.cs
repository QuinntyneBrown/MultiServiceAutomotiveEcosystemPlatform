using MediatR;
using Shared.Contracts.DTOs;

namespace Professional.Api.Queries.GetProfessional;

public record GetProfessionalQuery(Guid Id) : IRequest<ProfessionalDto?>;

public record GetProfessionalBySlugQuery(string Slug) : IRequest<ProfessionalDto?>;
