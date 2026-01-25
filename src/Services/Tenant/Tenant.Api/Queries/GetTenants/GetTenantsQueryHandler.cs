using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;
using Tenant.Core.Interfaces;

namespace Tenant.Api.Queries.GetTenants;

public class GetTenantsQueryHandler : IRequestHandler<GetTenantsQuery, PagedResult<TenantDto>>
{
    private readonly ITenantRepository _repository;

    public GetTenantsQueryHandler(ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<TenantDto>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var skip = (page - 1) * pageSize;

        var tenants = await _repository.GetAllAsync(skip, pageSize, cancellationToken);
        var totalCount = await _repository.GetCountAsync(cancellationToken);

        var items = tenants.Select(MapToDto).ToList();

        return PagedResult<TenantDto>.Create(items, totalCount, page, pageSize);
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
