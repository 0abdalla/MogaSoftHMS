using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Auth;

namespace Hospital_MS.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<ErrorResponseModel<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> RegisterAsync(RegisterUser request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default);

        Task<ErrorResponseModel<string>> SendResetPasswordCodeAsync(string userName);
        Task<ErrorResponseModel<string>> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
