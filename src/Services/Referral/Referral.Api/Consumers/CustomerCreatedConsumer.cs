using MassTransit;
using Referral.Infrastructure.Data;
using Shared.Contracts.Events.Customer;

namespace Referral.Api.Consumers;

public class CustomerCreatedConsumer : IConsumer<CustomerCreated>
{
    private readonly ReferralDbContext _context;
    private readonly ILogger<CustomerCreatedConsumer> _logger;

    public CustomerCreatedConsumer(ReferralDbContext context, ILogger<CustomerCreatedConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CustomerCreated> context)
    {
        var message = context.Message;

        _logger.LogInformation("Received CustomerCreated event for customer {CustomerId}", message.CustomerId);

        var existingCache = await _context.CustomerCache.FindAsync([message.CustomerId], context.CancellationToken);

        if (existingCache is null)
        {
            var customerCache = new CustomerCache
            {
                CustomerId = message.CustomerId,
                TenantId = message.TenantId,
                Email = message.Email,
                FullName = $"{message.FirstName} {message.LastName}",
                LastUpdated = DateTime.UtcNow
            };

            _context.CustomerCache.Add(customerCache);
            await _context.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("Cached customer {CustomerId} in Referral service", message.CustomerId);
        }
    }
}
