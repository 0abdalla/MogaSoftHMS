using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models;

public class TreasuryOperation
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ReceivedFrom { get; set; } // استلمت من السيد
    public decimal Amount { get; set; } // الوارد
    public bool IsActive { get; set; } = true;

    public int? AccountId { get; set; }
    public TransactionType TransactionType { get; set; }
    public int TreasuryId { get; set; }
    public Treasury Treasury { get; set; } = default!;

    public int? TreasuryMovementId { get; set; }
    public TreasuryMovement? TreasuryMovement { get; set; } = default!;
}