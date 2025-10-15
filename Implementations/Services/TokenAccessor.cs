using AuthSystem.Abstractions.IServices;

namespace AuthSystem.Implementations.Services
{
    public class TokenAccessor(IHttpContextAccessor httpContextAccessor) : ITokenAccessor
    {
        public string GetAuthenticatedTokenEmail()
        {
            return httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "Email")?.Value ?? throw new InvalidOperationException("Email claim not found.");
        }

        public string GetAuthenticatedTokenUsername()
        {
            return httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "UserName")?.Value ?? throw new InvalidOperationException("UserName claim not found.");
        }
    }
}
