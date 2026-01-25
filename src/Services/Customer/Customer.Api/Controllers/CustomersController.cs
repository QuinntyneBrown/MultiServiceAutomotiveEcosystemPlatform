using Customer.Api.Commands.AssignOwner;
using Customer.Api.Commands.CreateCustomer;
using Customer.Api.Commands.TransferOwnership;
using Customer.Api.Commands.UpdateCustomer;
using Customer.Api.Commands.UpdateCustomerStatus;
using Customer.Api.Commands.VerifyEmail;
using Customer.Api.Commands.VerifyPhone;
using Customer.Api.Queries.GetCustomer;
using Customer.Api.Queries.GetCustomers;
using Customer.Api.Queries.GetOwnershipHistory;
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

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerCommand(
            id,
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
        return Ok(result);
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateCustomerStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerStatusCommand(id, request.Status);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/assign-owner")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignOwner(Guid id, [FromBody] AssignOwnerRequest request, CancellationToken cancellationToken)
    {
        var command = new AssignOwnerCommand(id, request.ProfessionalId, request.Reason);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/transfer")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TransferOwnership(Guid id, [FromBody] TransferOwnershipRequest request, CancellationToken cancellationToken)
    {
        var command = new TransferOwnershipCommand(id, request.NewOwnerId, request.Reason);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/ownership-history")]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerOwnershipHistoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOwnershipHistory(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOwnershipHistoryQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/verify-email")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyEmail(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new VerifyEmailCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/verify-phone")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyPhone(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new VerifyPhoneCommand(id), cancellationToken);
        return Ok(result);
    }
}
