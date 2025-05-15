
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Auth;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Auth;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Services.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;

namespace Hospital_MS.Services.Auth
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider, ISQLHelper _sQLHelper,
        RoleManager<IdentityRole> _roleManager) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly ISQLHelper sQLHelper = _sQLHelper;
        private readonly RoleManager<IdentityRole> roleManager = _roleManager;

        public async Task<ErrorResponseModel<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByNameAsync(request.UserName) is not { } user)
                return ErrorResponseModel<AuthResponse>.Failure(GenericErrors.InvalidCredentials);

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            if (result.Succeeded)
            {
                var (token, expiresIn) = _jwtProvider.GenerateToken(user);

                var roles = await _userManager.GetRolesAsync(user);

                await _userManager.UpdateAsync(user);

                string roleId = null;

                if (!string.IsNullOrEmpty(roles.FirstOrDefault()))
                {
                    var role = await _roleManager.FindByNameAsync(roles?.FirstOrDefault());
                    roleId = role?.Id;
                }

                var Pages = GetPagesByRoleId(roleId);

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
                    expiresIn,
                    roles.FirstOrDefault(),
                    Pages.Select(i => i.PageName).ToList()
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

        public List<RolePages> GetPagesByRoleId(string RoleId)
        {
            var Params = new SqlParameter[1];
            Params[0] = new SqlParameter("@RoleId", RoleId);
            var dt = _sQLHelper.SQLQuery<RolePages>("[dbo].[SP_GetPagesByRoleId]", Params);
            return dt;
        }
    }
}
