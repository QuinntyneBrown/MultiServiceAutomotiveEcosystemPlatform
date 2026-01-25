using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;

namespace Tenant.Api.Queries.GetTenants;

public record GetTenantsQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<TenantDto>>;
