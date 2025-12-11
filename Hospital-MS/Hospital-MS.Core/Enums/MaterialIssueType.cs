using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums;

public enum MaterialIssueType
{

    [EnumMember(Value = "إرجاع")]
    Return,

    [EnumMember(Value = "تحويل لمخزن آخر")]
    Transfer,

    [EnumMember(Value = "هالك")]
    Waste,

    [EnumMember(Value = "صرف اقسام")]
    Department,

}
