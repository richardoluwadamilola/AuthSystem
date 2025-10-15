namespace AuthSystem.Abstractions.IServices
{
    public interface ITokenService
    {
        string GenerateToken(string username, string email);
    }
}
