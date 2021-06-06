using System.Threading.Tasks;
using app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IUserRepository _repository;

        public AuthController(IUserRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet("authorize")]
        public string AuthorizeRoute() =>
            "You are authorized to see this content";

        [HttpGet("unauthorize")]
        public string UnauthorizedRoute() =>
            "Anybody is allowed to see this content";

        // [HttpGet("token")]
        // public async Task<ActionResult<string>> GetToken() =>
            // Ok(await _repository.GetJWTTokenAsync());
    }
}