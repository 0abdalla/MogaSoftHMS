using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Auth
{
    public record RegisterRequest(
       string Email,
       string Password,
       string FirstName,
       string LastName,
       string? Address
   );
}
