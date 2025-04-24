using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums
{
    public enum Status
    {
        Success = 200,
        Failed = 400,
        Unauthorized = 401,
        NotFound = 404,
        Conflict = 409
    }
}
