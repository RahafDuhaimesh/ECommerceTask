namespace ECommerceTask.Application.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(string username, IList<string> roles);

    }
}
