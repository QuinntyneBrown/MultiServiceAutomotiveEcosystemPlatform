using MassTransit;
using Professional.Infrastructure.Data;
using Shared.Contracts.Events.Tenant;

namespace Professional.Api.Consumers;

public class TenantUpdatedConsumer : IConsumer<TenantUpdated>
{
    private readonly ProfessionalDbContext _context;
    private readonly ILogger<TenantUpdatedConsumer> _logger;

    public TenantUpdatedConsumer(ProfessionalDbContext context, ILogger<TenantUpdatedConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TenantUpdated> context)
    {
        var message = context.Message;

        _logger.LogInformation("Received TenantUpdated event for tenant {TenantId}", message.TenantId);

        var tenantCache = await _context.TenantCache.FindAsync([message.TenantId], context.CancellationToken);

        if (tenantCache is not null)
        {
            tenantCache.Name = message.Name;
            tenantCache.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("Updated tenant cache for {TenantId} in Professional service", message.TenantId);
        }
    }
}
