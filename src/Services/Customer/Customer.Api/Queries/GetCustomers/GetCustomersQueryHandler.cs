using Customer.Core.Interfaces;
using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;
using Shared.Infrastructure.Services;

namespace Customer.Api.Queries.GetCustomers;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, PagedResult<CustomerDto>>
{
    private readonly ICustomerRepository _repository;
    private readonly ITenantContext _tenantContext;

    public GetCustomersQueryHandler(ICustomerRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<PagedResult<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var skip = (page - 1) * pageSize;

        var customers = await _repository.GetByTenantAsync(_tenantContext.TenantId, skip, pageSize, cancellationToken);
        var totalCount = await _repository.GetCountByTenantAsync(_tenantContext.TenantId, cancellationToken);

        var items = customers.Select(MapToDto).ToList();

        return PagedResult<CustomerDto>.Create(items, totalCount, page, pageSize);
    }

    private static CustomerDto MapToDto(Core.Entities.Customer customer) => new()
    {
        Id = customer.Id,
        TenantId = customer.TenantId,
        Email = customer.Email,
        Phone = customer.Phone,
        FirstName = customer.FirstName,
        LastName = customer.LastName,
        MiddleName = customer.MiddleName,
        DateOfBirth = customer.DateOfBirth,
        StreetAddress = customer.StreetAddress,
        City = customer.City,
        State = customer.State,
        PostalCode = customer.PostalCode,
        Country = customer.Country,
        PreferredContactMethod = customer.PreferredContactMethod,
        MarketingOptIn = customer.MarketingOptIn,
        SmsOptIn = customer.SmsOptIn,
        Status = customer.Status,
        EmailVerified = customer.EmailVerified,
        PhoneVerified = customer.PhoneVerified,
        OwnerProfessionalId = customer.OwnerProfessionalId,
        CreatedAt = customer.CreatedAt,
        UpdatedAt = customer.UpdatedAt
    };
}
