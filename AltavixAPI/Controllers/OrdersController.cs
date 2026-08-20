using Altavix.Application.Features.Orders.Commands.CreateCart;
using Altavix.Application.Features.Orders.Commands.CheckoutOrder;
using Altavix.Application.Features.Orders.Commands.CreateOrder;
using Altavix.Application.Features.Orders.Commands.CancelOrder;
using Altavix.Application.Features.Orders.Commands.UpdateOrder;
using Altavix.Application.Features.Orders.Queries.GetOrderById;
using Altavix.Application.Features.Orders.Queries.GetOrdersList;
using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace AltavixAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a paginated list of orders, optionally filtered by a specific ClientId.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetOrdersList([FromQuery] GetOrdersListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific order by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetOrderByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Creates a new shopping cart for a given client (or a guest if ClientId is null).
    /// </summary>
    [HttpPost("cart")]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Checks out a cart, turning it into an order.
    /// </summary>
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new order directly (Legacy or used by Admins).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Cancels an order. Allowed only if not in processing.
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelOrderCommand command)
    {
        command.OrderId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Updates an order's client details. Allowed only if not in processing.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDetails(Guid id, [FromBody] UpdateOrderCommand command)
    {
        command.OrderId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Admin override to update an order's client details up to Processing state.
    /// </summary>
    [HttpPut("admin/{id}")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminUpdateDetails(Guid id, [FromBody] UpdateOrderCommand command)
    {
        command.OrderId = id;
        command.IsAdmin = true;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Updates an order's status.
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] Altavix.Application.Features.Orders.Commands.UpdateOrderStatus.UpdateOrderStatusCommand command)
    {
        command.OrderId = id;
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return Ok(result);
    }
}
