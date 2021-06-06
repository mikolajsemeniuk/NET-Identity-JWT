using System.Threading.Tasks;
using app.Payloads;

namespace app.Interfaces
{
    public interface ITodoRepository
    {
        Task<UserPayload> GetUserTodos(int id);
    }
}