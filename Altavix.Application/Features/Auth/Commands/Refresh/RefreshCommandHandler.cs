using Altavix.Domain.Repositories;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IJwtProvider _jwtProvider;

    public RefreshCommandHandler(
        UserManager<UserEntity> userManager,
        IJwtProvider jwtProvider, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }

    public async Task<ApiResponseDto<AuthResponseDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            return new ApiResponseDto<AuthResponseDto> { Message = "Відсутній refresh токен", Type = ResponseMessageType.Error };
        }

        var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == request.RefreshToken);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
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
                UserId = user.Id.ToString(),
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                MiddleName = user.MiddleName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            },
            Message = "Токен успішно оновлено",
            Type = ResponseMessageType.Success
        };
    }
}


