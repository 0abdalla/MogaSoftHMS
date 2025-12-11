using System.Runtime.Serialization;

namespace Hospital_MS.Core.Enums
{
    public enum PatientStatus
    {
        [EnumMember(Value = "نشط")]
        Active = 1,

        [EnumMember(Value = "غير نشط")]
        Inactive = 2,

        [EnumMember(Value = "طوارئ")]
        Emergency = 3,

        [EnumMember(Value = "عيادات خارجية")]
        Outpatient = 4,

        [EnumMember(Value = "منوم / إقامة داخلية")]
        Inpatient = 5,

        [EnumMember(Value = "متوفي")]
        Deceased = 6,

        [EnumMember(Value = "أرشيف")]
        Archived = 7
    }
}
