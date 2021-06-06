using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Data;
using app.Models;
using app.Payloads;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using app.Interfaces;

namespace app.Services
{
    public class TodoRepository : ITodoRepository
    {
        private readonly DataContext _context;
        private UserManager<User> _userManager;

        public TodoRepository(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserPayload> GetUserTodos(int id) =>
            await _userManager.Users
                .Where(user => user.Id == id)
                .Include(user => user.Todos)
                .Select(user => new UserPayload
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Todos = user.Todos.Select(todo => new TodoPayload
                    {
                        TodoId = todo.TodoId,
                        Name = todo.Name,
                        UserId = todo.UserId
                    }).ToList()
                })
                .SingleAsync();
    }
}