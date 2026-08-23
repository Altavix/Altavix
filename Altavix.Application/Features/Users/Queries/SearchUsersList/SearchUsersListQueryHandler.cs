        using Altavix.Application.Features.Users.DTOs;
using Altavix.Application.Features.Users.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;

namespace Altavix.Application.Features.Users.Queries.SearchUsersList;

public class SearchUsersListQueryHandler : BaseQueryHandler, IRequestHandler<SearchUsersListQuery, UsersListVm>
{
    public SearchUsersListQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<UsersListVm> Handle(SearchUsersListQuery request, CancellationToken cancellationToken)
    {
        var searchTerm = $"%{request.SearchTerm}%";
        
        const string sql = @"
            SELECT 
                Id, 
                Email, 
                FirstName, 
                LastName, 
                MiddleName, 
                PhoneNumber 
            FROM tbUsers
            WHERE FirstName LIKE @SearchTerm 
               OR LastName LIKE @SearchTerm 
               OR MiddleName LIKE @SearchTerm 
               OR PhoneNumber LIKE @SearchTerm 
               OR Email LIKE @SearchTerm";

        var users = await QueryAsync<UserDto>(sql, new { SearchTerm = searchTerm });

        return new UsersListVm
        {
            Users = users.ToList()
        };
    }
}
