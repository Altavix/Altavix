using Altavix.Application.Features.Brands.Commands.CreateBrand;
using Altavix.Application.Features.Brands.Commands.DeleteBrand;
using Altavix.Application.Features.Brands.Commands.UpdateBrand;
using Altavix.Application.Features.Brands.DTOs;
using Altavix.Application.Features.Brands.Queries.GetBrandById;
using Altavix.Application.Features.Brands.Queries.GetBrandsList;
using Altavix.Application.Features.Brands.ViewModels;
using Altavix.Application.Models;
using Altavix.Application.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AltavixAPI.Controllers;

public class BrandsController : BaseController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<BrandsListVm>>> Get([FromQuery] bool includeDisabled = true)
    {
        try
        {
            var vm = await Mediator.Send(new GetBrandsListQuery { IncludeDisabled = includeDisabled });
            return Ok(new ApiResponseDto<BrandsListVm> { Data = vm, Message = "Success", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<BrandsListVm> { Message = ex.Message, Type = ResponseMessageType.Error });
        }
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponseDto<BrandDto>>> Get(Guid id)
    {
        try
        {
            var brand = await Mediator.Send(new GetBrandByIdQuery { Id = id });
            if (brand == null) return NotFound(new ApiResponseDto<BrandDto> { Message = "Not found", Type = ResponseMessageType.Error });
            return Ok(new ApiResponseDto<BrandDto> { Data = brand, Message = "Success", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<BrandDto> { Message = ex.Message, Type = ResponseMessageType.Error });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ApiResponseDto<Guid>>> Create([FromBody] CreateBrandCommand command)
    {
        try
        {
            var id = await Mediator.Send(command);
            return Ok(new ApiResponseDto<Guid> { Data = id, Message = "Бренд успішно створено", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<Guid> { Message = ex.Message, Type = ResponseMessageType.Error });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponseDto<bool>>> Update(Guid id, [FromBody] UpdateBrandCommand command)
    {
        if (id != command.Id) return BadRequest(new ApiResponseDto<bool> { Message = "ID mismatch", Type = ResponseMessageType.Error });
        try
        {
            var success = await Mediator.Send(command);
            if (!success) return NotFound(new ApiResponseDto<bool> { Message = "Not found", Type = ResponseMessageType.Error });
            return Ok(new ApiResponseDto<bool> { Data = true, Message = "Бренд успішно оновлено", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<bool> { Message = ex.Message, Type = ResponseMessageType.Error });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponseDto<bool>>> Delete(Guid id)
    {
        try
        {
            var success = await Mediator.Send(new DeleteBrandCommand { Id = id });
            if (!success) return NotFound(new ApiResponseDto<bool> { Message = "Not found", Type = ResponseMessageType.Error });
            return Ok(new ApiResponseDto<bool> { Data = true, Message = "Бренд успішно видалено", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<bool> { Message = ex.Message, Type = ResponseMessageType.Error });
        }
    }
}
