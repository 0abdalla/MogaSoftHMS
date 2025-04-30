using System.Runtime.Serialization;

namespace Hospital_MS.Core.Enums
{
    public enum Gender
    {
        [EnumMember(Value = "ذكر")]
        Male,
        [EnumMember(Value = "أنثى")]
        Female,
    }
}
