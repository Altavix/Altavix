using Altavix.Application.Features.Users.DTOs;
using Altavix.Application.Features.Users.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;


namespace Altavix.Application.Features.Users.Queries.GetUsersList;

public class GetUsersListQueryHandler : BaseQueryHandler, IRequestHandler<GetUsersListQuery, UsersListVm>
{
    public GetUsersListQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<UsersListVm> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                Id, 
                Email, 
                FirstName, 
                LastName, 
                MiddleName, 
                PhoneNumber 
            FROM tbUsers";

        var users = await QueryAsync<UserDto>(sql);

        return new UsersListVm
        {
            Users = users.ToList()
        };
    }
}
