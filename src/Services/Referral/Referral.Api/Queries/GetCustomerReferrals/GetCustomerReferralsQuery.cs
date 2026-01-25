using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;

namespace Referral.Api.Queries.GetCustomerReferrals;

public record GetCustomerReferralsQuery(Guid CustomerId, int Page = 1, int PageSize = 10) : IRequest<PagedResult<CustomerReferralDto>>;
