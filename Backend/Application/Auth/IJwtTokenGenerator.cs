namespace Application.Auth
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string username, string role);
    }
}
