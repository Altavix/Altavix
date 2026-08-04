using Altavix.Domain;

namespace Altavix.Application.Interfaces;

public interface IJwtProvider
{
    string Generate(UserEntity user, IList<string> roles);
    string GenerateRefreshToken();
}
