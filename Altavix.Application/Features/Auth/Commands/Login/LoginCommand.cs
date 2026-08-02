using Altavix.Application.Features.Auth.DTOs;
using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<ApiResponseDto<AuthResponseDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
