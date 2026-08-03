using Altavix.Application.Features.Roles.Queries.GetRoleById;
using Altavix.Application.Features.Roles.Queries.GetRolesList;
using Altavix.Application.Features.Roles.ViewModels;
using Altavix.Application.Features.Roles.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AltavixAPI.Controllers;

[Authorize]
public class RoleController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<RolesListVm>> Get()
    {
        var query = new GetRolesListQuery();
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleVm>> GetById(Guid id)
    {
        var role = await Mediator.Send(new GetRoleByIdQuery(id));
        if (role == null) return NotFound();
        return Ok(role);
    }
}
