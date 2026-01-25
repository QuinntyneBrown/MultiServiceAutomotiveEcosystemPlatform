using MediatR;
using Shared.Contracts.DTOs;

namespace Tenant.Api.Commands.CreateTenant;

public record CreateTenantCommand(
    string Name,
    string? DisplayName,
    string? LogoUrl,
    string? PrimaryColor
) : IRequest<TenantDto>;
