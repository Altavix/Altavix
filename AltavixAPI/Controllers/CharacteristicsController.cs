using Altavix.Application.Features.Characteristics.Commands.CreateCharacteristic;
using Altavix.Application.Features.Characteristics.Commands.DeleteCharacteristic;
using Altavix.Application.Features.Characteristics.Commands.UpdateCharacteristic;
using Altavix.Application.Features.Characteristics.DTOs;
using Altavix.Application.Features.Characteristics.Queries.GetCharacteristicById;
using Altavix.Application.Features.Characteristics.Queries.GetCharacteristicsList;
using Altavix.Application.Features.Characteristics.ViewModels;
using Altavix.Application.Models;
using Altavix.Application.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AltavixAPI.Controllers;

public class CharacteristicsController : BaseController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<CharacteristicsListVm>>> Get([FromQuery] bool includeDisabled = true)
    {
        try
        {
            var vm = await Mediator.Send(new GetCharacteristicsListQuery { IncludeDisabled = includeDisabled });
            return Ok(new ApiResponseDto<CharacteristicsListVm> { Data = vm, Message = "Success", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<CharacteristicsListVm> { Message = ex.Message, Type = ResponseMessageType.Error });
        }
    }

    [AllowAnonymous]
    [HttpGet("filters")]
    public async Task<ActionResult<ApiResponseDto<List<Altavix.Application.Features.Characteristics.Queries.GetCharacteristicFilters.CharacteristicFilterDto>>>> GetFilters()
    {
        try
        {
            var filters = await Mediator.Send(new Altavix.Application.Features.Characteristics.Queries.GetCharacteristicFilters.GetCharacteristicFiltersQuery());
            return Ok(new ApiResponseDto<List<Altavix.Application.Features.Characteristics.Queries.GetCharacteristicFilters.CharacteristicFilterDto>> 
            { 
                Data = filters, 
                Message = "Success", 
                Type = ResponseMessageType.Success 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<List<Altavix.Application.Features.Characteristics.Queries.GetCharacteristicFilters.CharacteristicFilterDto>> { Message = ex.Message, Type = ResponseMessageType.Error });
        }
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<CharacteristicDto>>> Get(Guid id)
    {
        try
        {
            var characteristic = await Mediator.Send(new GetCharacteristicByIdQuery { Id = id });
            if (characteristic == null) return NotFound(new ApiResponseDto<CharacteristicDto> { Message = "Not found", Type = ResponseMessageType.Error });
            return Ok(new ApiResponseDto<CharacteristicDto> { Data = characteristic, Message = "Success", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<CharacteristicDto> { Message = ex.Message, Type = ResponseMessageType.Error });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ApiResponseDto<Guid>>> Create([FromBody] CreateCharacteristicCommand command)
    {
        try
        {
            var id = await Mediator.Send(command);
            return Ok(new ApiResponseDto<Guid> { Data = id, Message = "Характеристику успішно створено", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<Guid> { Message = ex.Message, Type = ResponseMessageType.Error });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<bool>>> Update(Guid id, [FromBody] UpdateCharacteristicCommand command)
    {
        if (id != command.Id) return BadRequest(new ApiResponseDto<bool> { Message = "ID mismatch", Type = ResponseMessageType.Error });
        try
        {
            var success = await Mediator.Send(command);
            if (!success) return NotFound(new ApiResponseDto<bool> { Message = "Not found", Type = ResponseMessageType.Error });
            return Ok(new ApiResponseDto<bool> { Data = true, Message = "Характеристику успішно оновлено", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<bool> { Message = ex.Message, Type = ResponseMessageType.Error });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<bool>>> Delete(Guid id)
    {
        try
        {
            var success = await Mediator.Send(new DeleteCharacteristicCommand { Id = id });
            if (!success) return NotFound(new ApiResponseDto<bool> { Message = "Not found", Type = ResponseMessageType.Error });
            return Ok(new ApiResponseDto<bool> { Data = true, Message = "Характеристику успішно видалено", Type = ResponseMessageType.Success });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponseDto<bool> { Message = ex.Message, Type = ResponseMessageType.Error });
        }
    }
}
