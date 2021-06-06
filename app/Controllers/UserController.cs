using System.Threading.Tasks;
using app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("signin/{email}/{password}")]
        public async Task<ActionResult> SignInUserAsync(string email, string password) =>
            Ok(new { Token = await _repository.SignInAsync(email, password) });

        [HttpPost("signup/{username}/{email}/{password}")]
        public async Task<ActionResult> SignUpUserAsync(string username, string email, string password) =>
            Ok(new { Token = await _repository.SignUpAsync(username, email, password) });

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetUsersAsync() =>
            Ok(await _repository.GetUserAsync());
    }
}