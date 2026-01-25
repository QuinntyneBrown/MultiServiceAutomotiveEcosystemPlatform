using MassTransit;
using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Tenant;
using Tenant.Core.Interfaces;
using Tenant.Infrastructure.Data;

namespace Tenant.Api.Commands.UpdateTenant;

public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, TenantDto>
{
    private readonly ITenantRepository _repository;
    private readonly TenantDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<UpdateTenantCommandHandler> _logger;

    public UpdateTenantCommandHandler(
        ITenantRepository repository,
        TenantDbContext context,
        IPublishEndpoint publishEndpoint,
        ILogger<UpdateTenantCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<TenantDto> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tenant with ID {request.Id} not found");

        tenant.Update(request.Name, request.DisplayName, request.LogoUrl, request.PrimaryColor);

        // Check for duplicate slug (excluding current tenant)
        if (await _repository.SlugExistsAsync(tenant.Slug, tenant.Id, cancellationToken))
        {
            throw new InvalidOperationException($"A tenant with slug '{tenant.Slug}' already exists");
        }

        _repository.Update(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new TenantUpdated(
            tenant.Id,
            tenant.Name,
            tenant.DisplayName,
            tenant.LogoUrl,
            tenant.PrimaryColor,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Updated tenant {TenantId}", tenant.Id);

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
