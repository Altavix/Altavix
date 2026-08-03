using Altavix.Application.Features.Products.Commands.CreateProduct;
using Altavix.Application.Features.Products.Commands.UpdateProduct;
using Altavix.Application.Features.Products.Commands.DeleteProduct;
using Altavix.Application.Features.Products.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Altavix.Application.Features.Products.Queries.GetProducts;
using Altavix.Application.Features.Products.Queries.GetProductById;
using Altavix.Application.Features.Products.ViewModels;

namespace AltavixAPI.Controllers;

public class ProductController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<ProductsListVm>> Get()
    {
        var query = new GetProductsListQuery();
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

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateProductCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteProductCommand { Id = id };
        await Mediator.Send(command);
        return NoContent();
    }
}
