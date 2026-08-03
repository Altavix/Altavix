using Altavix.Application.Features.Products.Commands.CreateProduct;
using Altavix.Application.Features.Products.Commands.UpdateProduct;
using Altavix.Application.Features.Products.Commands.DeleteProduct;
using Altavix.Application.Features.Products.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Altavix.Application.Features.Products.Queries.GetProducts;
using Altavix.Application.Features.Products.Queries.GetProductById;
using Altavix.Application.Features.Products.ViewModels;
using Altavix.Application.Models;
using System.Security.Claims;

namespace AltavixAPI.Controllers;

public class ProductController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProductVm>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetProductsListQuery(page, pageSize);
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PaginatedList<AdminProductVm>>> GetAdmin([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAdminProductsListQuery(page, pageSize);
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductVm>> GetById(Guid id)
    {
        var product = await Mediator.Send(new GetProductByIdQuery(id));
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpGet("admin/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AdminProductVm>> GetAdminById(Guid id)
    {
        var product = await Mediator.Send(new GetAdminProductByIdQuery(id));
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateProductCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            command.UserCreatorId = Guid.Parse(userId);
        }
        return await Mediator.Send(command);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update([FromBody] UpdateProductCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteProductCommand { Id = id };
        await Mediator.Send(command);
        return NoContent();
    }
}
