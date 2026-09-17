using Altavix.Application.Features.Categories.Commands.CreateCategory;
using Altavix.Application.Features.Categories.Commands.UpdateCategory;
using Altavix.Application.Features.Categories.Commands.DeleteCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Altavix.Application.Features.Categories.Queries.GetCategoriesList;
using Altavix.Application.Features.Categories.Queries.GetCategoryById;
using Altavix.Application.Features.Categories.ViewModels;
using Altavix.Application.Features.Categories.DTOs;
using Altavix.Application.Models;
using Altavix.Application.Enums;

namespace AltavixAPI.Controllers;

public class CategoryController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<CategoriesListVm>>> Get()
    {
        var query = new GetCategoriesListQuery();
        var result = await Mediator.Send(query);
        return Ok(new ApiResponseDto<CategoriesListVm> { Data = result, Message = "Success", Type = ResponseMessageType.Success });
    }

    [HttpGet("options")]
    public async Task<IActionResult> GetOptions()
    {
        var result = await Mediator.Send(new Altavix.Application.Features.Categories.Queries.GetCategoryOptions.GetCategoryOptionsQuery());
        return Ok(new ApiResponseDto<object> { Data = result, Message = "Success", Type = ResponseMessageType.Success });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponseDto<CategoryVm>>> GetById(Guid id)
    {
        var category = await Mediator.Send(new GetCategoryByIdQuery(id));
        if (category == null) return NotFound(new ApiResponseDto<CategoryVm> { Message = "Category not found", Type = ResponseMessageType.Error });
        return Ok(new ApiResponseDto<CategoryVm> { Data = category, Message = "Success", Type = ResponseMessageType.Success });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<Guid>>> Create([FromBody] CreateCategoryCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            return Ok(new ApiResponseDto<Guid> { Data = result, Message = "Category created successfully", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<Guid> { Message = $"Error: {ex.Message}", Type = ResponseMessageType.Error });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> Update([FromBody] UpdateCategoryCommand command)
    {
        try
        {
            await Mediator.Send(command);
            return Ok(new ApiResponseDto<bool> { Data = true, Message = "Category updated successfully", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<bool> { Message = $"Error: {ex.Message}", Type = ResponseMessageType.Error });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> Delete(Guid id)
    {
        try
        {
            var command = new DeleteCategoryCommand { Id = id };
            await Mediator.Send(command);
            return Ok(new ApiResponseDto<bool> { Data = true, Message = "Category deleted successfully", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<bool> { Message = $"Error: {ex.Message}", Type = ResponseMessageType.Error });
        }
    }
}
