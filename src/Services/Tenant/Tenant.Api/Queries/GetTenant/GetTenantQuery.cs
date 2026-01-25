using MediatR;
using Shared.Contracts.DTOs;

namespace Tenant.Api.Queries.GetTenant;

public record GetTenantQuery(Guid Id) : IRequest<TenantDto?>;

public record GetTenantBySlugQuery(string Slug) : IRequest<TenantDto?>;
