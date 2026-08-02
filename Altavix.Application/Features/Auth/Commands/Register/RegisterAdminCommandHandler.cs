using Altavix.Domain;
using Altavix.Application.Models;
using Altavix.Application.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Altavix.Application.Features.Auth.Commands.Register;

public class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand, ApiResponseDto<Guid>>
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IConfiguration _configuration;

    public RegisterAdminCommandHandler(UserManager<UserEntity> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<ApiResponseDto<Guid>> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return new ApiResponseDto<Guid> { Message = "Паролі не співпадають", Type = ResponseMessageType.Error };
        }

        var secretKey = _configuration["AdminRegistrationKey"];
        if (string.IsNullOrEmpty(secretKey) || request.SecretKey != secretKey)
        {
            return new ApiResponseDto<Guid> { Message = "Невірний Secret Key для реєстрації адміністратора", Type = ResponseMessageType.Error };
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
            return new ApiResponseDto<Guid> { Message = $"Помилка створення адміна: {errors}", Type = ResponseMessageType.Error };
        }

        await _userManager.AddToRoleAsync(user, "Admin");

        return new ApiResponseDto<Guid>
        {
            Data = user.Id,
            Message = "Адміністратора успішно зареєстровано",
            Type = ResponseMessageType.Success
        };
    }
}
