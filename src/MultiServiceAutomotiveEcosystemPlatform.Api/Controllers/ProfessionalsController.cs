// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiServiceAutomotiveEcosystemPlatform.Api.Features.Professionals;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfessionalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfessionalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new professional
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProfessionalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfessionalDto>> CreateProfessional([FromBody] CreateProfessionalCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProfessional), new { id = result.ProfessionalId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get a professional by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProfessionalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfessionalDto>> GetProfessional(Guid id)
    {
        var result = await _mediator.Send(new GetProfessionalQuery { ProfessionalId = id });
        
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Get a list of professionals (paginated)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetProfessionalsQueryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetProfessionalsQueryResponse>> GetProfessionals([FromQuery] int page = 1, [FromQuery] int pageSize = 30)
    {
        var result = await _mediator.Send(new GetProfessionalsQuery { Page = page, PageSize = pageSize });
        return Ok(result);
    }
}
