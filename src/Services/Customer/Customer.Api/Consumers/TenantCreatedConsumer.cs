using Customer.Infrastructure.Data;
using MassTransit;
using Shared.Contracts.Events.Tenant;

namespace Customer.Api.Consumers;

public class TenantCreatedConsumer : IConsumer<TenantCreated>
{
    private readonly CustomerDbContext _context;
    private readonly ILogger<TenantCreatedConsumer> _logger;

    public TenantCreatedConsumer(CustomerDbContext context, ILogger<TenantCreatedConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TenantCreated> context)
    {
        var message = context.Message;

        _logger.LogInformation("Received TenantCreated event for tenant {TenantId}", message.TenantId);

        var existingCache = await _context.TenantCache.FindAsync([message.TenantId], context.CancellationToken);

        if (existingCache is null)
        {
            var tenantCache = new TenantCache
            {
                TenantId = message.TenantId,
                Name = message.Name,
                Slug = message.Slug,
                LastUpdated = DateTime.UtcNow
            };

            _context.TenantCache.Add(tenantCache);
            await _context.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("Cached tenant {TenantId} in Customer service", message.TenantId);
        }
    }
}
