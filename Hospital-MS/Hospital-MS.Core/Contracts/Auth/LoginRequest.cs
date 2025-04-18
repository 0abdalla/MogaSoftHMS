using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Auth
{
    public record LoginRequest(
        string Email,
        string Password
    );
   
}
