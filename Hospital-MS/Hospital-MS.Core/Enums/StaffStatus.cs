using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums
{
    public enum StaffStatus
    {
        [EnumMember(Value = "نشط")]
        Active,
        [EnumMember(Value = "غير نشط")]
        Inactive,
        [EnumMember(Value = "مفصول")]
        Suspended,
        [EnumMember(Value = "مستقيل")]
        OnLeave
    }
}
