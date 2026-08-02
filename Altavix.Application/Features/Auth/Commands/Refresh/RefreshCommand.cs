using Altavix.Application.Features.Auth.DTOs;
using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.Auth.Commands.Refresh;

public class RefreshCommand : IRequest<ApiResponseDto<AuthResponseDto>>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
