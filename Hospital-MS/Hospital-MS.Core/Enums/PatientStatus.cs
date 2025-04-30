using System.Runtime.Serialization;

namespace Hospital_MS.Core.Enums
{
    public enum PatientStatus
    {
        [EnumMember(Value = "أرشيف")]
        Archived,

        [EnumMember(Value = "معالج")]
        Treated,

        [EnumMember(Value = "حالة حرجة")]
        CriticalCondition,

        [EnumMember(Value = "جراحة")]
        Surgery,

        [EnumMember(Value = "متابعة")]
        FollowUp,

        [EnumMember(Value = "إقامة")]
        Staying,

        [EnumMember(Value = "عيادات خارجية")]
        Outpatient,

        [EnumMember(Value = "عناية مركزة")]
        IntensiveCare,

        [EnumMember(Value = "طوارئ")]
        Emergency,

        [EnumMember(Value = "حضانات الأطفال")]
        NeonatalCare,
    }
}
