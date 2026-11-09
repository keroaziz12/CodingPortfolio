using CodingPortfolio.Models;

namespace CodingPortfolio.Services
{
    public interface IAuthService
    {
        Task<Developer?> RegisterAsync(string name, string email, string password);
        Task<string?> LoginAsync(string email, string password);
    }
}