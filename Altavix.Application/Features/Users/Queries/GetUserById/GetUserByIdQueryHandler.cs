using Altavix.Application.Features.Users.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;


namespace Altavix.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetUserByIdQuery, UserVm?>
{
    public GetUserByIdQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<UserVm?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                Id, 
                UserName, 
                Email, 
                FirstName, 
                LastName 
            FROM tbUsers 
            WHERE Id = @Id";

        return await QueryFirstOrDefaultAsync<UserVm>(sql, new { request.Id });
    }
}

