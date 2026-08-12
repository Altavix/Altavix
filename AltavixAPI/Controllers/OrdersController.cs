using Altavix.Application.Features.Orders.Commands.CreateCart;
using Altavix.Application.Features.Orders.Commands.CheckoutOrder;
using Altavix.Application.Features.Orders.Commands.CreateOrder;
using Altavix.Application.Features.Orders.Queries.GetOrderById;
using Altavix.Application.Features.Orders.Queries.GetOrdersList;
using MediatR;
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
    /// Gets a list of orders (supports filtering by date and client).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetOrdersListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets detailed information for a specific order by ID.
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
    /// Creates an empty cart (an order with Ordered = null).
    /// </summary>
    [HttpPost("cart")]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Finalizes the cart into a real order by providing delivery and client details.
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
}

