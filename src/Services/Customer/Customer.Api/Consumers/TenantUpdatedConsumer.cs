using Customer.Infrastructure.Data;
using MassTransit;
using Shared.Contracts.Events.Tenant;

namespace Customer.Api.Consumers;

public class TenantUpdatedConsumer : IConsumer<TenantUpdated>
{
    private readonly CustomerDbContext _context;
    private readonly ILogger<TenantUpdatedConsumer> _logger;

    public TenantUpdatedConsumer(CustomerDbContext context, ILogger<TenantUpdatedConsumer> logger)
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

            _logger.LogInformation("Updated tenant cache for {TenantId} in Customer service", message.TenantId);
        }
    }
}
