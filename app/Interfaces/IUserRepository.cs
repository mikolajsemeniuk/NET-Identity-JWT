using System.Threading.Tasks;
using app.Models;

namespace app.Interfaces
{
    public interface IUserRepository
    {
        Task<string> GetJWTTokenAsync(User user);
        Task<string> SignUpAsync(string username, string email, string password);
        Task<string> SignInAsync(string email, string password);
    }
}