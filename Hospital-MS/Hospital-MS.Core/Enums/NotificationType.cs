using System.Runtime.Serialization;

namespace Hospital_MS.Core.Enums;
public enum NotificationType
{
    [EnumMember(Value = "طلب شراء")]
    PurchaseRequest,
    [EnumMember(Value = "طلب صرف")]
    DisbursementRequest
}
