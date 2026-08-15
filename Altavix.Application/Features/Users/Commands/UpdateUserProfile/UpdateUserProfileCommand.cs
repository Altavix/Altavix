using Altavix.Application.Models;
using MediatR;
using System.Text.Json.Serialization;

namespace Altavix.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommand : IRequest<ApiResponseDto<bool>>
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
