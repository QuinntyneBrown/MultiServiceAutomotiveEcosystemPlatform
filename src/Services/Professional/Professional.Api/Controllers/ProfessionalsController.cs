using MediatR;
using Microsoft.AspNetCore.Mvc;
using Professional.Api.Commands.CreateProfessional;
using Professional.Api.Queries.GetProfessional;
using Professional.Api.Queries.GetProfessionals;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;

namespace Professional.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfessionalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfessionalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProfessionalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProfessionalRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProfessionalCommand(
            request.BusinessName,
            request.BusinessDescription,
            request.BusinessPhone,
            request.BusinessEmail,
            request.Website,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.StreetAddress,
            request.City,
            request.State,
            request.PostalCode,
            request.Country,
            request.Latitude,
            request.Longitude,
            request.AcceptsReferrals,
            request.ReferralCommissionRate);

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProfessionalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProfessionalQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("slug/{slug}")]
    [ProducesResponseType(typeof(ProfessionalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProfessionalBySlugQuery(slug), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProfessionalDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetProfessionalsQuery(page, pageSize), cancellationToken);
        return Ok(result);
    }
}
