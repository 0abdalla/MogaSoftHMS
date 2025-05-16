using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<ErrorResponseModel<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<string>> RegisterAsync(RegisterUser request, CancellationToken cancellationToken = default);
    }
}
