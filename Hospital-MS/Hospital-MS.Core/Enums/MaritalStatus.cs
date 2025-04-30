using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums
{
    public enum MaritalStatus
    {
        [EnumMember(Value = "أعزب")]
        Single,
        [EnumMember(Value = "متزوج")]
        Married,
        [EnumMember(Value = "مطلق")]
        Divorced,
        [EnumMember(Value = "أرمل")]
        Widowed,
        [EnumMember(Value = "منفصل")]
        Separated
    }
}
