using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;

namespace Professional.Api.Queries.GetProfessionals;

public record GetProfessionalsQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<ProfessionalDto>>;
