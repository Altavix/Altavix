using Altavix.Application.Features.Users.ViewModels;
using Altavix.Application.Features.Users.Queries.GetUsersList;
using Altavix.Application.Features.Users.Queries.GetUserById;
using Altavix.Application.Features.Users.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("{id}")]
    public async Task<ActionResult<UserVm>> GetById(Guid id)
    {
        var user = await Mediator.Send(new GetUserByIdQuery(id));
        if (user == null) return NotFound();
        return Ok(user);
    }
}
