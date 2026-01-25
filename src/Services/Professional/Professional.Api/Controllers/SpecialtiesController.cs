using MediatR;
using Microsoft.AspNetCore.Mvc;
using Professional.Api.Queries.GetSpecialties;
using Shared.Contracts.DTOs;

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
}
