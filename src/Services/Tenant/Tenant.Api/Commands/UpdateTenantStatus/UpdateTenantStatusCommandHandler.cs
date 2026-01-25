using MassTransit;
using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Enums;
using Shared.Contracts.Events.Tenant;
using Tenant.Core.Interfaces;
using Tenant.Infrastructure.Data;

namespace Tenant.Api.Commands.UpdateTenantStatus;

public class UpdateTenantStatusCommandHandler : IRequestHandler<UpdateTenantStatusCommand, TenantDto>
{
    private readonly ITenantRepository _repository;
    private readonly TenantDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<UpdateTenantStatusCommandHandler> _logger;

    public UpdateTenantStatusCommandHandler(
        ITenantRepository repository,
        TenantDbContext context,
        IPublishEndpoint publishEndpoint,
        ILogger<UpdateTenantStatusCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<TenantDto> Handle(UpdateTenantStatusCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tenant with ID {request.Id} not found");

        var previousStatus = tenant.Status;

        switch (request.Status)
        {
            case TenantStatus.Active:
                tenant.Activate();
                break;
            case TenantStatus.Suspended:
                tenant.Suspend();
                break;
            case TenantStatus.Inactive:
                tenant.Deactivate();
                break;
            default:
                throw new ArgumentException($"Invalid status: {request.Status}");
        }

        _repository.Update(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new TenantStatusChanged(
            tenant.Id,
            previousStatus.ToString(),
            tenant.Status.ToString(),
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Updated tenant {TenantId} status from {PreviousStatus} to {NewStatus}",
            tenant.Id, previousStatus, tenant.Status);

        return MapToDto(tenant);
    }

    private static TenantDto MapToDto(Core.Entities.Tenant tenant) => new()
    {
        Id = tenant.Id,
        Name = tenant.Name,
        Slug = tenant.Slug,
        DisplayName = tenant.DisplayName,
        LogoUrl = tenant.LogoUrl,
        PrimaryColor = tenant.PrimaryColor,
        Status = tenant.Status,
        CreatedAt = tenant.CreatedAt,
        UpdatedAt = tenant.UpdatedAt
    };
}
