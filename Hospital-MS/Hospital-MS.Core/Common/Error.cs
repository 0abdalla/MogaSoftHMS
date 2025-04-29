using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Common
{
    public record Error(string Message, Status? Status)
    {
        public static readonly Error None = new(string.Empty, null);
    }
}
