using Altavix.Domain;
using Altavix.Application.Models;
using Altavix.Application.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Altavix.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponseDto<Guid>>
{
    private readonly UserManager<UserEntity> _userManager;

    public RegisterCommandHandler(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiResponseDto<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return new ApiResponseDto<Guid> { Message = "Паролі не співпадають", Type = ResponseMessageType.Error };
        }
        
        var user = new UserEntity
        {
            Email = request.Email,
            UserName = Guid.NewGuid().ToString(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            PhoneNumber = request.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new ApiResponseDto<Guid> { Message = $"Помилка реєстрації: {errors}", Type = ResponseMessageType.Error };
        }

        await _userManager.AddToRoleAsync(user, "User");

        return new ApiResponseDto<Guid>
        {
            Data = user.Id,
            Message = "Реєстрація успішна",
            Type = ResponseMessageType.Success
        };
    }
}
