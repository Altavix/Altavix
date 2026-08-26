using Altavix.Application.Features.Users.ViewModels;
using Altavix.Application.Features.Users.Queries.GetUsersList;
using Altavix.Application.Features.Users.Queries.GetUserById;
using Altavix.Application.Features.Users.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Altavix.Application.Features.Users.Commands.UpdateUserProfile;
using System.Security.Claims;

namespace AltavixAPI.Controllers;

[Authorize]
public class UserController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<UsersListVm>> Get()
    {
        var query = new GetUsersListQuery();
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<UsersListVm>> Search([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term)) return BadRequest("Search term is required");
        var query = new Altavix.Application.Features.Users.Queries.SearchUsersList.SearchUsersListQuery(term);
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserVm>> GetById(Guid id)
    {
        var user = await Mediator.Send(new GetUserByIdQuery(id));
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPut("profile")]
    public async Task<ActionResult> UpdateProfile([FromBody] UpdateUserProfileCommand command)
    {
        try
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            command.UserId = userId;
            
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
