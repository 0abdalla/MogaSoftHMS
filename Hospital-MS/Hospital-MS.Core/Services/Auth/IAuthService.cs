using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services.Auth
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    }
}
