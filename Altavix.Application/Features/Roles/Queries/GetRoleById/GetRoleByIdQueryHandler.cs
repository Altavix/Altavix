using Altavix.Application.Features.Roles.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;


namespace Altavix.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetRoleByIdQuery, RoleVm?>
{
    public GetRoleByIdQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<RoleVm?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                Id, 
                Name 
            FROM tbRoles 
            WHERE Id = @Id";

        return await QueryFirstOrDefaultAsync<RoleVm>(sql, new { request.Id });
    }
}

