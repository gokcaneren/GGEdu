using GGEdu.Core.Entities.Users;

namespace GGEdu.Core.Services.Users
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        string HashRefreshToken(string refreshToken);
    }
}
