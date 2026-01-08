// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiServiceAutomotiveEcosystemPlatform.Api.Features.Customers;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCustomer), new { id = result.CustomerId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get a customer by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetCustomer(Guid id)
    {
        var result = await _mediator.Send(new GetCustomerQuery { CustomerId = id });
        
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Get a list of customers (paginated)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetCustomersQueryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetCustomersQueryResponse>> GetCustomers([FromQuery] int page = 1, [FromQuery] int pageSize = 30)
    {
        var result = await _mediator.Send(new GetCustomersQuery { Page = page, PageSize = pageSize });
        return Ok(result);
    }
}
