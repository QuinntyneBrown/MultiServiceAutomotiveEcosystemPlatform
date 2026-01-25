using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Enums;

namespace Tenant.Api.Commands.UpdateTenantStatus;

public record UpdateTenantStatusCommand(Guid Id, TenantStatus Status) : IRequest<TenantDto>;
