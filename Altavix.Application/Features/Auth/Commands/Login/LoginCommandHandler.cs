using Altavix.Application.Features.Auth.DTOs;
using Altavix.Application.Interfaces;
using Altavix.Application.Models;
using Altavix.Application.Enums;
using Altavix.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Altavix.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponseDto<AuthResponseDto>>
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(
        UserManager<UserEntity> userManager,
        IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }

    public async Task<ApiResponseDto<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new ApiResponseDto<AuthResponseDto> { Message = "Невірний email або пароль", Type = ResponseMessageType.Error };
        }

        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isValidPassword)
        {
            return new ApiResponseDto<AuthResponseDto> { Message = "Невірний email або пароль", Type = ResponseMessageType.Error };
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtProvider.Generate(user, roles);
        var refreshToken = _jwtProvider.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // 7 days valid
        await _userManager.UpdateAsync(user);

        return new ApiResponseDto<AuthResponseDto>
        {
            Data = new AuthResponseDto
            {
                Email = user.Email ?? string.Empty,
                Token = token,
                RefreshToken = refreshToken
            },
            Message = "Успішний вхід",
            Type = ResponseMessageType.Success
        };
    }
}
