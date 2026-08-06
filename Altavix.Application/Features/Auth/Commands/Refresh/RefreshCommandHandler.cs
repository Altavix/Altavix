using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Altavix.Application.Features.Auth.DTOs;
using Altavix.Application.Interfaces;
using Altavix.Application.Models;
using Altavix.Application.Enums;
using Altavix.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Altavix.Application.Features.Auth.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, ApiResponseDto<AuthResponseDto>>
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IJwtProvider _jwtProvider;

    public RefreshCommandHandler(
        UserManager<UserEntity> userManager,
        IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }

    public async Task<ApiResponseDto<AuthResponseDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(request.AccessToken))
        {
            return new ApiResponseDto<AuthResponseDto> { Message = "Невірний access токен", Type = ResponseMessageType.Error };
        }

        var jwtToken = handler.ReadJwtToken(request.AccessToken);
        var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

        if (string.IsNullOrEmpty(emailClaim))
        {
            return new ApiResponseDto<AuthResponseDto> { Message = "Невірні дані у токені", Type = ResponseMessageType.Error };
        }

        var user = await _userManager.FindByEmailAsync(emailClaim);
        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new ApiResponseDto<AuthResponseDto> { Message = "Недійсний або прострочений токен", Type = ResponseMessageType.Error };
        }

        var roles = await _userManager.GetRolesAsync(user);

        var newAccessToken = _jwtProvider.Generate(user, roles);
        var newRefreshToken = _jwtProvider.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new ApiResponseDto<AuthResponseDto>
        {
            Data = new AuthResponseDto
            {
                Email = user.Email ?? string.Empty,
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Role = roles.FirstOrDefault() ?? string.Empty,
                UserId = user.Id.ToString()
            },
            Message = "Токен успішно оновлено",
            Type = ResponseMessageType.Success
        };
    }
}
