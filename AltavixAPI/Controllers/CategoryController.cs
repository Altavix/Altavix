using Altavix.Application.Features.Categories.Commands.CreateCategory;
using Altavix.Application.Features.Categories.Commands.UpdateCategory;
using Altavix.Application.Features.Categories.Commands.DeleteCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Altavix.Application.Features.Categories.Queries.GetCategoriesList;
using Altavix.Application.Features.Categories.Queries.GetCategoryById;
using Altavix.Application.Features.Categories.ViewModels;
using Altavix.Application.Features.Categories.DTOs;

namespace AltavixAPI.Controllers;

public class CategoryController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<CategoriesListVm>> Get()
    {
        var query = new GetCategoriesListQuery();
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryVm>> GetById(Guid id)
    {
        var category = await Mediator.Send(new GetCategoryByIdQuery(id));
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateCategoryCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteCategoryCommand { Id = id };
        await Mediator.Send(command);
        return NoContent();
    }
}
