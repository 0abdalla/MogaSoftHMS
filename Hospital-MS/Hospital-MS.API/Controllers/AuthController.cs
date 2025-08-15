using Hospital_MS.Core.Contracts.Auth;
using Hospital_MS.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;
using ResetPasswordRequest = Hospital_MS.Core.Contracts.Auth.ResetPasswordRequest;

namespace Hospital_MS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Core.Contracts.Auth.LoginRequest request, CancellationToken cancellationToken)
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

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var authResult = await _authService.SendResetPasswordCodeAsync(request.UserName);

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var authResult = await _authService.ResetPasswordAsync(request);

            return Ok();
        }
    }
}
