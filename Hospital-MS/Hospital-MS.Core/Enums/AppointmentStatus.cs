using System.Runtime.Serialization;

namespace Hospital_MS.Core.Enums
{
    public enum AppointmentStatus
    {
        [EnumMember(Value = "معلق")]
        Pending,
        [EnumMember(Value = "ملغي")]
        Rejected,
        [EnumMember(Value = "مكتمل")]
        Completed
    }
}
