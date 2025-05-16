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
       string UserName,
       string? Address
   );

    public record RegisterUser(
      int? StaffId,
      string FirstName,
      string LastName,
      string Email,
      string Password,
      string? Address,
      string RoleName
  );
}
