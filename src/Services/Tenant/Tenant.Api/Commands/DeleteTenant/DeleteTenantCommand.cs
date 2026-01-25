using MediatR;

namespace Tenant.Api.Commands.DeleteTenant;

public record DeleteTenantCommand(Guid Id) : IRequest<bool>;
