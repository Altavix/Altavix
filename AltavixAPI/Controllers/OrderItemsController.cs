using Altavix.Application.Features.OrderItems.Commands.AddOrderItem;
using Altavix.Application.Features.OrderItems.Commands.CancelOrderItem;
using Altavix.Application.Features.OrderItems.Commands.MarkOrderItemReadyToShip;
using Altavix.Application.Features.OrderItems.Commands.RemoveOrderItem;
using Altavix.Application.Features.OrderItems.Commands.ShipOrderItem;
using Altavix.Application.Features.OrderItems.Commands.UpdateOrderItemQuantity;
using Altavix.Application.Features.OrderItems.Queries.GetOrderItemsByOrderId;
using Altavix.Application.Features.OrderItems.Queries.GetOrderItemsByStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AltavixAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all items for a specific order.
    /// </summary>
    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetByOrderId(Guid orderId)
    {
        var result = await _mediator.Send(new GetOrderItemsByOrderIdQuery { OrderId = orderId });
        return Ok(result);
    }

    /// <summary>
    /// Gets order items by status (useful for warehouse workers).
    /// </summary>
    [HttpGet("status")]
    public async Task<IActionResult> GetByStatus([FromQuery] GetOrderItemsByStatusQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Adds a new item to an order (cart).
    /// </summary>
    [HttpPost("add")]
    public async Task<IActionResult> AddItem([FromBody] AddOrderItemCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Updates the quantity of an order item.
    /// </summary>
    [HttpPut("update-quantity")]
    public async Task<IActionResult> UpdateQuantity([FromBody] UpdateOrderItemQuantityCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Removes an item from an order.
    /// </summary>
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveItem([FromBody] RemoveOrderItemCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Marks an order item as ready to ship.
    /// </summary>
    [HttpPatch("mark-ready")]
    public async Task<IActionResult> MarkReadyToShip([FromBody] MarkOrderItemReadyToShipCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Marks an order item as shipped.
    /// </summary>
    [HttpPatch("ship")]
    public async Task<IActionResult> ShipItem([FromBody] ShipOrderItemCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Cancels an order item (requires a reason).
    /// </summary>
    [HttpPatch("cancel")]
    public async Task<IActionResult> CancelItem([FromBody] CancelOrderItemCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
