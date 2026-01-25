using MassTransit;
using MediatR;
using Shared.Contracts.Events.Tenant;
using Tenant.Core.Interfaces;
using Tenant.Infrastructure.Data;

namespace Tenant.Api.Commands.DeleteTenant;

public class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand, bool>
{
    private readonly ITenantRepository _repository;
    private readonly TenantDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<DeleteTenantCommandHandler> _logger;

    public DeleteTenantCommandHandler(
        ITenantRepository repository,
        TenantDbContext context,
        IPublishEndpoint publishEndpoint,
        ILogger<DeleteTenantCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tenant with ID {request.Id} not found");

        // Soft delete by marking as deleted (if supported) or hard delete
        _repository.Delete(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new TenantDeleted(
            tenant.Id,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Deleted tenant {TenantId} ({TenantName})", tenant.Id, tenant.Name);

        return true;
    }
}
