using Application.Users.Features.Login;
using Application.Users.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : Controller
    {
        private readonly LoginService _login;

        public AuthController(LoginService login) => _login = login;

        [HttpPost("login")]
        public async Task<IActionResult> Login(
         LoginRequest request,
         CancellationToken ct)
        {
            var result = await _login.LoginAsync(request, ct);
            return result is null ? Unauthorized() : Ok(result);
        }
    }
}
