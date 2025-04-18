using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Abstractions
{
    public record Error(string Code, string Message, int? statusCode)
    {
        public static readonly Error None = new(string.Empty, string.Empty, null);
    }
}
