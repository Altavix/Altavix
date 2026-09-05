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
using Altavix.Application.Enums;
using System.Security.Claims;

namespace AltavixAPI.Controllers;

public class ProductController : BaseController
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponseDto<PaginatedList<ProductVm>>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 8,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] Guid[]? brandIds = null,
        [FromQuery] Guid[]? categoryIds = null,
        [FromQuery] string? characteristicsJson = null,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? sortBy = null)
    {
        var dict = new Dictionary<Guid, string[]>();
        if (!string.IsNullOrEmpty(characteristicsJson))
        {
            try { dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<Guid, string[]>>(characteristicsJson) ?? dict; } catch { }
        }

        var query = new GetProductsListQuery(page, pageSize, minPrice, maxPrice, brandIds, categoryIds, dict)
        {
            SearchTerm = searchTerm,
            SortBy = sortBy
        };
        var result = await Mediator.Send(query);
        return Ok(new ApiResponseDto<PaginatedList<ProductVm>> { Data = result, Message = "Success", Type = ResponseMessageType.Success });
    }

    [HttpGet("max-price")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponseDto<decimal>>> GetMaxPrice()
    {
        var result = await Mediator.Send(new Altavix.Application.Features.Products.Queries.GetMaxPrice.GetMaxPriceQuery());
        return Ok(new ApiResponseDto<decimal> { Data = result, Message = "Success", Type = ResponseMessageType.Success });
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<PaginatedList<AdminProductVm>>>> GetAdmin(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] Guid[]? brandIds = null,
        [FromQuery] Guid[]? categoryIds = null,
        [FromQuery] string? characteristicsJson = null,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? sortBy = null)
    {
        var dict = new Dictionary<Guid, string[]>();
        if (!string.IsNullOrEmpty(characteristicsJson))
        {
            try { dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<Guid, string[]>>(characteristicsJson) ?? dict; } catch { }
        }

        var result = await Mediator.Send(new GetAdminProductsListQuery(page, pageSize)
        {
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            BrandIds = brandIds,
            CategoryIds = categoryIds,
            CharacteristicsFilters = dict,
            SearchTerm = searchTerm,
            SortBy = sortBy
        });
        return Ok(new ApiResponseDto<PaginatedList<AdminProductVm>> { Data = result, Message = "Success", Type = ResponseMessageType.Success });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponseDto<ProductVm>>> GetById(Guid id)
    {
        var product = await Mediator.Send(new GetProductByIdQuery(id));
        if (product == null) 
            return NotFound(new ApiResponseDto<ProductVm> { Message = "Product not found", Type = ResponseMessageType.Error });
            
        return Ok(new ApiResponseDto<ProductVm> { Data = product, Message = "Success", Type = ResponseMessageType.Success });
    }

    [HttpGet("admin/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<AdminProductVm>>> GetAdminById(Guid id)
    {
        var product = await Mediator.Send(new GetAdminProductByIdQuery(id));
        if (product == null) 
            return NotFound(new ApiResponseDto<AdminProductVm> { Message = "Product not found", Type = ResponseMessageType.Error });
            
        return Ok(new ApiResponseDto<AdminProductVm> { Data = product, Message = "Success", Type = ResponseMessageType.Success });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<Guid>>> Create([FromBody] CreateProductCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
        if (!string.IsNullOrEmpty(userId))
        {
            command.UserCreatorId = Guid.Parse(userId);
        }
        else
        {
            return Unauthorized(new ApiResponseDto<Guid> { Message = "User ID is missing from the token.", Type = ResponseMessageType.Error });
        }
        
        try 
        {
            var result = await Mediator.Send(command);
            return Ok(new ApiResponseDto<Guid> { Data = result, Message = "Product created successfully", Type = ResponseMessageType.Success });
        } 
        catch (Exception ex) 
        {
            return StatusCode(500, new ApiResponseDto<Guid> { Message = $"Error: {ex.Message}", Type = ResponseMessageType.Error });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> Update([FromBody] UpdateProductCommand command)
    {
        try 
        {
            await Mediator.Send(command);
            return Ok(new ApiResponseDto<bool> { Data = true, Message = "Product updated successfully", Type = ResponseMessageType.Success });
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
            var command = new DeleteProductCommand { Id = id };
            await Mediator.Send(command);
            return Ok(new ApiResponseDto<bool> { Data = true, Message = "Product deleted successfully", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<bool> { Message = $"Error: {ex.Message}", Type = ResponseMessageType.Error });
        }
    }
}
