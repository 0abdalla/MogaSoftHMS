using Hospital_MS.Core.Contracts.Auth;
using Hospital_MS.Interfaces.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.LoginAsync(request, cancellationToken);
            return Ok(authResult);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.RegisterAsync(request, cancellationToken);
            return Ok(authResult);
        }

        [HttpPost("UpdateUserAsync")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
        {
            var authResult = await _authService.UpdateUserAsync(request, cancellationToken);
            return Ok(authResult);
        }
    }
}
