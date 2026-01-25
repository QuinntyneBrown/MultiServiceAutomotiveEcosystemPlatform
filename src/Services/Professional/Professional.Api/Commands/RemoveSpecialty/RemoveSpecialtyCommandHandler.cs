using MassTransit;
using MediatR;
using Professional.Core.Interfaces;
using Professional.Infrastructure.Data;
using Shared.Contracts.Events.Professional;
using Shared.Infrastructure.Services;

namespace Professional.Api.Commands.RemoveSpecialty;

public class RemoveSpecialtyCommandHandler : IRequestHandler<RemoveSpecialtyCommand, bool>
{
    private readonly IProfessionalRepository _repository;
    private readonly ProfessionalDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RemoveSpecialtyCommandHandler> _logger;

    public RemoveSpecialtyCommandHandler(
        IProfessionalRepository repository,
        ProfessionalDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<RemoveSpecialtyCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<bool> Handle(RemoveSpecialtyCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var professional = await _repository.GetByIdAsync(request.ProfessionalId, cancellationToken)
            ?? throw new KeyNotFoundException($"Professional with ID {request.ProfessionalId} not found");

        var specialty = professional.Specialties.FirstOrDefault(s => s.SpecialtyId == request.SpecialtyId);
        if (specialty is null)
            return false;

        professional.RemoveSpecialty(request.SpecialtyId);

        _repository.Update(professional);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new ProfessionalSpecialtyRemoved(
            professional.Id,
            professional.TenantId,
            request.SpecialtyId,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Removed specialty {SpecialtyId} from professional {ProfessionalId}",
            request.SpecialtyId, professional.Id);

        return true;
    }
}
