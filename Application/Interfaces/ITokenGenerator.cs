namespace ECommerceTask.Application.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(int userId, string username, IList<string> roles);

    }
}
