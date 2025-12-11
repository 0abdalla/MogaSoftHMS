using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Enums
{
    public enum AdmissionType
    {
        [EnumMember(Value = "طوارئ")]
        Emergency,

        [EnumMember(Value = "دخول مخطط")]
        Elective,

        [EnumMember(Value = "جراحة")]
        Surgery,

        [EnumMember(Value = "باطنة")]
        Medical,

        [EnumMember(Value = "أطفال")]
        Pediatric,

        [EnumMember(Value = "ولادة")]
        Maternity,

        [EnumMember(Value = "تحويل من العناية المركزة")]
        ICUTransfer,

        [EnumMember(Value = "تحويل من الطوارئ")]
        ERTransfer,

        [EnumMember(Value = "ملاحظة")]
        Observation
    }
}
