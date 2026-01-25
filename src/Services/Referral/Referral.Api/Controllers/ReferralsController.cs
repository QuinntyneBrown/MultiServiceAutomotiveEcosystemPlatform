using MediatR;
using Microsoft.AspNetCore.Mvc;
using Referral.Api.Commands.AcceptProfessionalReferral;
using Referral.Api.Commands.ConvertCustomerReferral;
using Referral.Api.Commands.CreateCustomerReferral;
using Referral.Api.Commands.CreateProfessionalReferral;
using Referral.Api.Commands.DeclineProfessionalReferral;
using Referral.Api.Commands.GenerateReferralCode;
using Referral.Api.Commands.UpdateCustomerReferralStatus;
using Referral.Api.Commands.UseReferralCode;
using Referral.Api.Commands.ValidateReferralCode;
using Referral.Api.Queries.GetCustomerReferralDetails;
using Referral.Api.Queries.GetCustomerReferrals;
using Referral.Api.Queries.GetProfessionalReferralDetails;
using Referral.Api.Queries.GetProfessionalReferrals;
using Referral.Api.Queries.GetReferralCode;
using Referral.Api.Queries.GetReferralCodesByOwner;
using Referral.Api.Queries.GetReferralStats;
using Referral.Api.Queries.GetReferralStatsSummary;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;

namespace Referral.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReferralsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReferralsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Customer Referrals

    [HttpPost("customer")]
    [ProducesResponseType(typeof(CustomerReferralDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomerReferral([FromBody] CreateCustomerReferralRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCustomerReferralCommand(
            request.ReferringCustomerId,
            request.ReferredEmail,
            request.ReferredPhone,
            request.ReferredFirstName,
            request.ReferredLastName,
            request.ReferralSource,
            request.ReferralCode,
            request.Notes);

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCustomerReferrals), new { customerId = result.ReferringCustomerId }, result);
    }

    [HttpGet("customer/{customerId:guid}")]
    [ProducesResponseType(typeof(PagedResult<CustomerReferralDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomerReferrals(
        Guid customerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetCustomerReferralsQuery(customerId, page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("customer/{customerId:guid}/referrals/{referralId:guid}")]
    [ProducesResponseType(typeof(CustomerReferralDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerReferralDetails(
        Guid customerId,
        Guid referralId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomerReferralDetailsQuery(referralId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPatch("customer/{customerId:guid}/referrals/{referralId:guid}/status")]
    [ProducesResponseType(typeof(CustomerReferralDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCustomerReferralStatus(
        Guid customerId,
        Guid referralId,
        [FromBody] UpdateCustomerReferralStatusRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerReferralStatusCommand(referralId, request.Status);
        var result = await _mediator.Send(command, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("customer/{customerId:guid}/referrals/{referralId:guid}/convert")]
    [ProducesResponseType(typeof(CustomerReferralDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConvertCustomerReferral(
        Guid customerId,
        Guid referralId,
        [FromBody] ConvertCustomerReferralRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ConvertCustomerReferralCommand(referralId, request.NewCustomerId, request.RewardAmount, request.RewardType);
        var result = await _mediator.Send(command, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    // Professional Referrals

    [HttpPost("professional")]
    [ProducesResponseType(typeof(ProfessionalReferralDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProfessionalReferral([FromBody] CreateProfessionalReferralRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProfessionalReferralCommand(
            request.ReferringProfessionalId,
            request.ReferredBusinessName,
            request.ReferredEmail,
            request.ReferredPhone,
            request.ContactPersonName,
            request.ReferralSource,
            request.ReferralCode,
            request.Notes);

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetProfessionalReferrals), new { professionalId = result.ReferringProfessionalId }, result);
    }

    [HttpGet("professional/{professionalId:guid}")]
    [ProducesResponseType(typeof(PagedResult<ProfessionalReferralDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfessionalReferrals(
        Guid professionalId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetProfessionalReferralsQuery(professionalId, page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("professional/{professionalId:guid}/referrals/{referralId:guid}")]
    [ProducesResponseType(typeof(ProfessionalReferralDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfessionalReferralDetails(
        Guid professionalId,
        Guid referralId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProfessionalReferralDetailsQuery(referralId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("professional/{professionalId:guid}/referrals/{referralId:guid}/accept")]
    [ProducesResponseType(typeof(ProfessionalReferralDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AcceptProfessionalReferral(
        Guid professionalId,
        Guid referralId,
        CancellationToken cancellationToken)
    {
        var command = new AcceptProfessionalReferralCommand(referralId);
        var result = await _mediator.Send(command, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("professional/{professionalId:guid}/referrals/{referralId:guid}/decline")]
    [ProducesResponseType(typeof(ProfessionalReferralDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeclineProfessionalReferral(
        Guid professionalId,
        Guid referralId,
        [FromBody] DeclineProfessionalReferralRequest? request,
        CancellationToken cancellationToken)
    {
        var command = new DeclineProfessionalReferralCommand(referralId, request?.Reason);
        var result = await _mediator.Send(command, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    // Referral Codes

    [HttpPost("codes")]
    [ProducesResponseType(typeof(ReferralCodeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateReferralCode([FromBody] GenerateReferralCodeRequest request, CancellationToken cancellationToken)
    {
        var command = new GenerateReferralCodeCommand(
            request.CustomerId,
            request.ProfessionalId,
            request.CampaignName,
            request.ExpiresAt,
            request.MaxUses);

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetReferralCode), new { code = result.Code }, result);
    }

    [HttpGet("codes/{code}")]
    [ProducesResponseType(typeof(ReferralCodeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReferralCode(string code, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetReferralCodeQuery(code), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("codes/owner/{ownerId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<ReferralCodeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReferralCodesByOwner(
        Guid ownerId,
        [FromQuery] string ownerType = "customer",
        CancellationToken cancellationToken = default)
    {
        var customerId = ownerType.Equals("customer", StringComparison.OrdinalIgnoreCase) ? ownerId : (Guid?)null;
        var professionalId = ownerType.Equals("professional", StringComparison.OrdinalIgnoreCase) ? ownerId : (Guid?)null;
        var result = await _mediator.Send(new GetReferralCodesByOwnerQuery(customerId, professionalId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("codes/validate")]
    [ProducesResponseType(typeof(ReferralCodeValidationResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateReferralCode(
        [FromBody] ValidateReferralCodeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ValidateReferralCodeCommand(request.Code), cancellationToken);
        return Ok(result);
    }

    [HttpPost("codes/{code}/use")]
    [ProducesResponseType(typeof(ReferralCodeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UseReferralCode(string code, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UseReferralCodeCommand(code), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    // Stats

    [HttpGet("stats/customer/{customerId:guid}")]
    [ProducesResponseType(typeof(ReferralStatsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerStats(Guid customerId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetReferralStatsQuery(customerId, null), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("stats/professional/{professionalId:guid}")]
    [ProducesResponseType(typeof(ReferralStatsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfessionalStats(Guid professionalId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetReferralStatsQuery(null, professionalId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("stats/summary")]
    [ProducesResponseType(typeof(ReferralStatsSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReferralStatsSummary(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetReferralStatsSummaryQuery(), cancellationToken);
        return Ok(result);
    }
}
