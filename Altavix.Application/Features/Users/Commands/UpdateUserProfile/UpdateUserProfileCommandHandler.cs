using Altavix.Application.Enums;
using Altavix.Application.Models;
using Altavix.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Altavix.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ApiResponseDto<bool>>
{
    private readonly UserManager<UserEntity> _userManager;

    public UpdateUserProfileCommandHandler(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiResponseDto<bool>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            return new ApiResponseDto<bool>
            {
                Type = ResponseMessageType.Error,
                Message = "Користувача не знайдено",
                Data = false
            };
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.MiddleName = request.MiddleName;
        user.PhoneNumber = request.PhoneNumber;

        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            // Update email
            var emailToken = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);
            var changeEmailResult = await _userManager.ChangeEmailAsync(user, request.Email, emailToken);
            
            if (!changeEmailResult.Succeeded)
            {
                return new ApiResponseDto<bool>
                {
                    Type = ResponseMessageType.Error,
                    Message = "Помилка при зміні Email: " + string.Join(", ", changeEmailResult.Errors.Select(e => e.Description)),
                    Data = false
                };
            }

            // Sync UserName with Email
            var setUserNameResult = await _userManager.SetUserNameAsync(user, request.Email);
            if (!setUserNameResult.Succeeded)
            {
                return new ApiResponseDto<bool>
                {
                    Type = ResponseMessageType.Error,
                    Message = "Помилка при оновленні логіна: " + string.Join(", ", setUserNameResult.Errors.Select(e => e.Description)),
                    Data = false
                };
            }
        }

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            return new ApiResponseDto<bool>
            {
                Type = ResponseMessageType.Error,
                Message = "Помилка при оновленні профілю: " + string.Join(", ", updateResult.Errors.Select(e => e.Description)),
                Data = false
            };
        }

        return new ApiResponseDto<bool>
        {
            Type = ResponseMessageType.Success,
            Message = "Профіль успішно оновлено",
            Data = true
        };
    }
}
