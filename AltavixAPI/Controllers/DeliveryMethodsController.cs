using Altavix.Application.Features.DeliveryMethods.Commands.CreateDeliveryMethod;
using Altavix.Application.Features.DeliveryMethods.Commands.ToggleDeliveryMethodStatus;
using Altavix.Application.Features.DeliveryMethods.Queries.GetActiveDeliveryMethods;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AltavixAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryMethodsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeliveryMethodsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all active delivery methods.
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _mediator.Send(new GetActiveDeliveryMethodsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Gets active delivery methods formatted for select dropdowns.
    /// </summary>
    [HttpGet("options")]
    public async Task<IActionResult> GetOptions()
    {
        var result = await _mediator.Send(new Altavix.Application.Features.DeliveryMethods.Queries.GetDeliveryMethodOptions.GetDeliveryMethodOptionsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Creates a new delivery method.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDeliveryMethodCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Toggles the active status of a delivery method.
    /// </summary>
    [HttpPatch("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var command = new ToggleDeliveryMethodStatusCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
