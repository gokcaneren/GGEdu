using GGEdu.Application.Helpers;
using GGEdu.Core.Entities.Users;
using GGEdu.Core.Services.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GGEdu.Application.Services.Users
{
    public class AuthService : IAuthService
    {
        private readonly TokenConfiguration _tokenConfiguration;

        public AuthService(TokenConfiguration tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var randomNumGenerator = RandomNumberGenerator.Create();
            randomNumGenerator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public string HashRefreshToken(string refreshToken)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(refreshToken);
            var hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        public string GenerateToken(User user)
        {
            var claims = CreateClaims(user);
            var creds = CreateCredentials();

            var token = BuildSecurityToken(claims, creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private List<Claim> CreateClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            return claims;
        }


        private SigningCredentials CreateCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return creds;
        }

        private JwtSecurityToken BuildSecurityToken(List<Claim> claims, SigningCredentials creds)
        {
            return new JwtSecurityToken(
                issuer: _tokenConfiguration.Issuer,
                audience: _tokenConfiguration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_tokenConfiguration.ExpiryTime),
                signingCredentials: creds
            );
        }
    }
}
