using Altavix.Application.Features.Roles.DTOs;
using Altavix.Application.Features.Roles.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;


namespace Altavix.Application.Features.Roles.Queries.GetRolesList;

public class GetRolesListQueryHandler : BaseQueryHandler, IRequestHandler<GetRolesListQuery, RolesListVm>
{
    public GetRolesListQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<RolesListVm> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                Id, 
                Name 
            FROM tbRoles";

        var roles = await QueryAsync<RoleDto>(sql);

        return new RolesListVm
        {
            Roles = roles.ToList()
        };
    }
}
