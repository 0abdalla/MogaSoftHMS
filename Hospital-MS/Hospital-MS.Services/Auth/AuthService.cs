using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Auth;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Hospital_MS.Services.Auth
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Failure<AuthResponse>(GenericErrors<ApplicationUser>.InvalidCredentials);

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            if (result.Succeeded)
            {
                var (token, expiresIn) = _jwtProvider.GenerateToken(user);

                await _userManager.UpdateAsync(user);

                var response = new AuthResponse(
                    user.Id,
                    user.Email!,
                    user.FirstName,
                    user.LastName,
                    user.Address,
                    user.IsActive,
                    user.LoginDate,
                    token,
                    expiresIn
                );

                return Result.Success(response);
            }

            return Result.Failure<AuthResponse>(GenericErrors<ApplicationUser>.InvalidCredentials);
        }

        public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var emailIsExists = await _userManager.FindByEmailAsync(request.Email) is not null;

            if (emailIsExists)
                return Result.Failure(GenericErrors<ApplicationUser>.DuplicateEmail);

           var user = new ApplicationUser
           {
               FirstName = request.FirstName,
               LastName = request.LastName,
               Address = request.Address,
               Email = request.Email,
               UserName = request.Email,
               IsActive = true,
           };

            user.UserName = request.Email;

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return Result.Success();
            }

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
    }
}
