using MediatR;
using Microsoft.AspNetCore.Mvc;
using Professional.Api.Commands.CreateSpecialty;
using Professional.Api.Queries.GetSpecialties;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;

namespace Professional.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialtiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpecialtiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SpecialtyCatalogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSpecialtiesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SpecialtyCatalogDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSpecialtyRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSpecialtyCommand(request.Name, request.Description, request.Category);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAll), result);
    }
}
