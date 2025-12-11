using System.Runtime.Serialization;

namespace Hospital_MS.Core.Enums;

public enum AppointmentType
{
    [EnumMember(Value = "كشف")]
    General,

    [EnumMember(Value = "استشارة")]
    Consultation,

    [EnumMember(Value = "عمليات")]
    Surgery,

    [EnumMember(Value = "تحاليل")]
    Lab,

    [EnumMember(Value = "أشعة")]
    Radiology,
}
