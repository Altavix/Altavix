using Altavix.Application.Features.Users.ViewModels;
using MediatR;

namespace Altavix.Application.Features.Users.Queries.SearchUsersList;

public record SearchUsersListQuery(string SearchTerm) : IRequest<UsersListVm>;
