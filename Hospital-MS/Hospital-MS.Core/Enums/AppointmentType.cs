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


        MRI,            // أشعة رنين
        Panorama,       // بانوراما
        CTScan,         // مقطعية
        Ultrasound,     // سونار
        XRay,           // عادية
        Echo,           // إيكو
        Mammogram       // مامو جرام
    }
}
