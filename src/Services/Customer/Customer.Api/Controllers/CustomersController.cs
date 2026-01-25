using Customer.Api.Commands.CreateCustomer;
using Customer.Api.Queries.GetCustomer;
using Customer.Api.Queries.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;

namespace Customer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCustomerCommand(
            request.Email,
            request.Phone,
            request.FirstName,
            request.LastName,
            request.MiddleName,
            request.DateOfBirth,
            request.StreetAddress,
            request.City,
            request.State,
            request.PostalCode,
            request.Country,
            request.PreferredContactMethod,
            request.MarketingOptIn,
            request.SmsOptIn);

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomerQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetCustomersQuery(page, pageSize), cancellationToken);
        return Ok(result);
    }
}
