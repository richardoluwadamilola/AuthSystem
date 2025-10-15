using AuthSystem.Abstractions.IServices;
using AuthSystem.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthSystem.Implementations.Services
{
    public class TokenService(IOptions<JwtSettings> jwtSettings) : ITokenService
    {
        private readonly JwtSettings jwtSettings = jwtSettings.Value;

        public string GenerateToken(string username, string email)
        {
            List<Claim> authClaims =
            [
                new Claim("UserName", username),
                new Claim("Email", email)
            ];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.ValidIssuer,
                audience: jwtSettings.ValidAudience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
