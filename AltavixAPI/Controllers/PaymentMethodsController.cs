using Altavix.Application.Features.PaymentMethods.Commands.CreatePaymentMethod;
using Altavix.Application.Features.PaymentMethods.Commands.TogglePaymentMethodStatus;
using Altavix.Application.Features.PaymentMethods.Queries.GetActivePaymentMethods;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AltavixAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentMethodsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentMethodsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all active payment methods.
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _mediator.Send(new GetActivePaymentMethodsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Gets active payment methods formatted for select dropdowns.
    /// </summary>
    [HttpGet("options")]
    public async Task<IActionResult> GetOptions()
    {
        var result = await _mediator.Send(new Altavix.Application.Features.PaymentMethods.Queries.GetPaymentMethodOptions.GetPaymentMethodOptionsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Creates a new payment method.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentMethodCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Toggles the active status of a payment method.
    /// </summary>
    [HttpPatch("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var command = new TogglePaymentMethodStatusCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
