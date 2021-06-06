using System;
using System.Security.Claims;
using System.Threading.Tasks;
using app.Interfaces;
using app.Payloads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    public class TodoController : BaseController
    {
        private readonly ITodoRepository _repository;

        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserPayload>> GetUserAsync()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _repository.GetUserTodos(Convert.ToInt32(id)));
        }
    }
}