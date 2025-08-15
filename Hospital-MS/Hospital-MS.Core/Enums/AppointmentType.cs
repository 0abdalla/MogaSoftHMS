using System.Runtime.Serialization;

namespace Hospital_MS.Core.Enums
{
    public enum AppointmentType
    {
        [EnumMember(Value = "كشف")]
        General,
        [EnumMember(Value = "استشارة")]
        Consultation,
        [EnumMember(Value = "عمليات")]
        Surgery,
        [EnumMember(Value = "تحاليل")]
        Screening,
        [EnumMember(Value = "أشعة")]
        Radiology,
        [EnumMember(Value = "طوارئ")]
        Emergency,

        [EnumMember(Value = "أشعة")]
        MRI,
        [EnumMember(Value = "أشعة")]
        Panorama,
        [EnumMember(Value = "أشعة")]
        CTScan,
        [EnumMember(Value = "أشعة")]
        Ultrasound,
        [EnumMember(Value = "أشعة")]
        XRay,
        [EnumMember(Value = "أشعة")]
        Echo,
        [EnumMember(Value = "أشعة")]
        Mammogram
    }
}
