

using AuthSystem.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthSystem.Utilities.Token
{
    public static class TokenUtil
    {
        private static JwtSettings? _jwtSettings;
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Configure(JwtSettings jwtSettings, IHttpContextAccessor httpContextAccessor)
        {
            _jwtSettings = jwtSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        public static string GetAuthenticatedTokenEmail()
        {
            var emailClaim = _httpContextAccessor?.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "Email");
            return emailClaim?.Value ?? throw new InvalidOperationException("Email claim not found.");
        }

        public static string GetAuthenticatedTokenUsername()
        {
            var userNameClaim = _httpContextAccessor?.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "UsernName");
            return userNameClaim?.Value ?? throw new InvalidOperationException("UserName claim not found.");
        }

        public static string GetToken(string userName, string email)
        {
            try
            {
                if (_jwtSettings == null)
                    throw new InvalidOperationException("TokenUtil is not configured.");
                var JwtSettings = _jwtSettings;
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey));
                SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                List<Claim> authClaims = new List<Claim>
                {
                    new Claim("UserName", userName),
                    new Claim("Email", email)
                };

                JwtSecurityToken token = new JwtSecurityToken(JwtSettings.ValidIssuer, //Issuer    
                                JwtSettings.ValidAudience,  //Audience
                                expires: DateTime.Now.AddHours(3),
                                claims: authClaims,
                                signingCredentials: credentials);
                string jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt_token;
            }

            catch (Exception ex)
            {
                throw new Exception("Error generating token", ex);
            }
        }

        public static ClaimDetails? DecryptToken(string token)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                if (jwtSecurityToken == null)
                    return null;
                return new ClaimDetails { UserName = jwtSecurityToken.Claims.First(claim => claim.Type == "UserName").Value, Email = jwtSecurityToken.Claims.First(claim => claim.Type == "Email").Value };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
