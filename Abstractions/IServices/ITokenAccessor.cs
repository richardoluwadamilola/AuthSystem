namespace AuthSystem.Abstractions.IServices
{
    public interface ITokenAccessor
    {
        string GetAuthenticatedTokenEmail();
        string GetAuthenticatedTokenUsername();
    }
}
