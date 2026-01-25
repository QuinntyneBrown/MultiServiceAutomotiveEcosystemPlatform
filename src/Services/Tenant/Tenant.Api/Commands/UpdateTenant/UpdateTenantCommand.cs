using MediatR;
using Shared.Contracts.DTOs;

namespace Tenant.Api.Commands.UpdateTenant;

public record UpdateTenantCommand(
    Guid Id,
    string Name,
    string? DisplayName,
    string? LogoUrl,
    string? PrimaryColor
) : IRequest<TenantDto>;
