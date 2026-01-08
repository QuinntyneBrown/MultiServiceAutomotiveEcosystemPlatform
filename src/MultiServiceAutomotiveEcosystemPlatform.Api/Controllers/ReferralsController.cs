// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiServiceAutomotiveEcosystemPlatform.Api.Features.Referrals;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReferralsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReferralsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new customer referral
    /// </summary>
    [HttpPost("customer")]
    [ProducesResponseType(typeof(CustomerReferralDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerReferralDto>> CreateCustomerReferral([FromBody] CreateCustomerReferralCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Created($"/api/referrals/customer/{result.ReferrerCustomerId}", result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get customer referrals
    /// </summary>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(GetCustomerReferralsQueryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetCustomerReferralsQueryResponse>> GetCustomerReferrals(
        Guid customerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 30)
    {
        var result = await _mediator.Send(new GetCustomerReferralsQuery
        {
            CustomerId = customerId,
            Page = page,
            PageSize = pageSize
        });
        return Ok(result);
    }

    /// <summary>
    /// Create a new professional referral
    /// </summary>
    [HttpPost("professional")]
    [ProducesResponseType(typeof(ProfessionalReferralDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfessionalReferralDto>> CreateProfessionalReferral([FromBody] CreateProfessionalReferralCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Created($"/api/referrals/professional/{result.SourceProfessionalId}", result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get professional referrals (sent or received)
    /// </summary>
    [HttpGet("professional/{professionalId}")]
    [ProducesResponseType(typeof(GetProfessionalReferralsQueryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetProfessionalReferralsQueryResponse>> GetProfessionalReferrals(
        Guid professionalId,
        [FromQuery] string direction = "sent",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 30)
    {
        var result = await _mediator.Send(new GetProfessionalReferralsQuery
        {
            ProfessionalId = professionalId,
            Direction = direction,
            Page = page,
            PageSize = pageSize
        });
        return Ok(result);
    }
}
