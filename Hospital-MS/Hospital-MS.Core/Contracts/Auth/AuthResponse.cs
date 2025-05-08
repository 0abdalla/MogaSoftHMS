using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Auth
{
    public record AuthResponse(
        string UserId,
        string Email,
        string FirstName,
        string LastName,
        string? Address,
        bool IsActive,
        DateTime? LoginDate,
        string UserName,
        string Token,
        int ExpiresIn
    );

}
