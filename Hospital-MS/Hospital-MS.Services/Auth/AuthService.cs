
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Auth;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Auth;
using Hospital_MS.Services.Common;
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

        public async Task<ErrorResponseModel<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByNameAsync(request.UserName) is not { } user)
                return ErrorResponseModel<AuthResponse>.Failure(GenericErrors.InvalidCredentials);

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
                    user.UserName!,
                    token,
                    expiresIn
                );

                return ErrorResponseModel<AuthResponse>.Success(GenericErrors.SuccessLogin, response);
            }

            return ErrorResponseModel<AuthResponse>.Failure(GenericErrors.InvalidCredentials);
        }

        public async Task<ErrorResponseModel<string>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var emailIsExists = await _userManager.FindByEmailAsync(request.Email) is not null;

            if (emailIsExists)
                return ErrorResponseModel<string>.Failure(GenericErrors.DuplicateEmail);

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                Email = request.Email,
                //UserName = request.FirstName+" "+request.LastName,
                UserName = request.UserName,
                IsActive = true,
            };

            //user.UserName = request.Email;

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return ErrorResponseModel<string>.Success(GenericErrors.SuccessRegister);
            }

            var error = result.Errors.First();

            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }
}
