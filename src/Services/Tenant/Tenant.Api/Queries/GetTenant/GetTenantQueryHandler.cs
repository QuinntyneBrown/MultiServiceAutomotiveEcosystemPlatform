using MediatR;
using Shared.Contracts.DTOs;
using Tenant.Core.Interfaces;

namespace Tenant.Api.Queries.GetTenant;

public class GetTenantQueryHandler : IRequestHandler<GetTenantQuery, TenantDto?>
{
    private readonly ITenantRepository _repository;

    public GetTenantQueryHandler(ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<TenantDto?> Handle(GetTenantQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return tenant is null ? null : MapToDto(tenant);
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

public class GetTenantBySlugQueryHandler : IRequestHandler<GetTenantBySlugQuery, TenantDto?>
{
    private readonly ITenantRepository _repository;

    public GetTenantBySlugQueryHandler(ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<TenantDto?> Handle(GetTenantBySlugQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _repository.GetBySlugAsync(request.Slug, cancellationToken);
        return tenant is null ? null : MapToDto(tenant);
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
