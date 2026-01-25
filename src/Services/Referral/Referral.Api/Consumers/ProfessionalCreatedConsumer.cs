using MassTransit;
using Referral.Infrastructure.Data;
using Shared.Contracts.Events.Professional;

namespace Referral.Api.Consumers;

public class ProfessionalCreatedConsumer : IConsumer<ProfessionalCreated>
{
    private readonly ReferralDbContext _context;
    private readonly ILogger<ProfessionalCreatedConsumer> _logger;

    public ProfessionalCreatedConsumer(ReferralDbContext context, ILogger<ProfessionalCreatedConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProfessionalCreated> context)
    {
        var message = context.Message;

        _logger.LogInformation("Received ProfessionalCreated event for professional {ProfessionalId}", message.ProfessionalId);

        var existingCache = await _context.ProfessionalCache.FindAsync([message.ProfessionalId], context.CancellationToken);

        if (existingCache is null)
        {
            var professionalCache = new ProfessionalCache
            {
                ProfessionalId = message.ProfessionalId,
                TenantId = message.TenantId,
                BusinessName = message.BusinessName,
                Email = message.Email,
                LastUpdated = DateTime.UtcNow
            };

            _context.ProfessionalCache.Add(professionalCache);
            await _context.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("Cached professional {ProfessionalId} in Referral service", message.ProfessionalId);
        }
    }
}
