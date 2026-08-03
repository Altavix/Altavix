using MediatR;
using Microsoft.AspNetCore.Mvc;
using Altavix.Application.Models;
using Altavix.Application.Enums;

namespace AltavixAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    private IMediator? _mediator;
    
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

    protected ObjectResult HandleResult<T>(ApiResponseDto<T> result)
    {
        if (result.Type == ResponseMessageType.Error)
            return BadRequest(result);
            
        return Ok(result);
    }

    protected ObjectResult HandleError(Exception ex)
    {
        return BadRequest(new ApiResponseDto<object> 
        { 
            Data = null, 
            Message = ex.Message, 
            Type = ResponseMessageType.Error 
        });
    }
}
