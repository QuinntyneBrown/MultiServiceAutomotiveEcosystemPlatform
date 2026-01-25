using MassTransit;
using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Tenant;
using Tenant.Core.Interfaces;
using Tenant.Infrastructure.Data;

namespace Tenant.Api.Commands.CreateTenant;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, TenantDto>
{
    private readonly ITenantRepository _repository;
    private readonly TenantDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CreateTenantCommandHandler> _logger;

    public CreateTenantCommandHandler(
        ITenantRepository repository,
        TenantDbContext context,
        IPublishEndpoint publishEndpoint,
        ILogger<CreateTenantCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<TenantDto> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = new Core.Entities.Tenant(request.Name, request.DisplayName);

        if (!string.IsNullOrEmpty(request.LogoUrl) || !string.IsNullOrEmpty(request.PrimaryColor))
        {
            tenant.Update(request.Name, request.DisplayName, request.LogoUrl, request.PrimaryColor);
        }

        // Check for duplicate slug
        if (await _repository.SlugExistsAsync(tenant.Slug, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException($"A tenant with slug '{tenant.Slug}' already exists");
        }

        await _repository.AddAsync(tenant, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new TenantCreated(
            tenant.Id,
            tenant.Name,
            tenant.Slug,
            tenant.DisplayName,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Created tenant {TenantId} with slug {Slug}", tenant.Id, tenant.Slug);

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
