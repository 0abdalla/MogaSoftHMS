using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums;

public enum EmergencySeverity
{
    [EnumMember(Value = "تدخل فوري")]
    Level1_Resuscitation,
    [EnumMember(Value = "خطيرة")]
    Level2_Emergent = 2,
    [EnumMember(Value = "عاجلة")]
    Level3_Urgent = 3,
    [EnumMember(Value = "متوسطة")]
    Level4_LessUrgent = 4,
    [EnumMember(Value = "بسيطة")]
    Level5_NonUrgent = 5
}
