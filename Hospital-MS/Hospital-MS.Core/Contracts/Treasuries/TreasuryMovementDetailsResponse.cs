namespace Hospital_MS.Core.Contracts.Treasuries;
public class TreasuryMovementDetailsResponse
{
    public int MovementId { get; set; }
    public string TreasuryName { get; set; } = string.Empty;
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal TotalReceipts { get; set; }
    public decimal TotalPayments { get; set; }
    public decimal ClosingBalance { get; set; }
    public List<TreasuryTransactionRow> Receipts { get; set; } = new();
    public List<TreasuryTransactionRow> Payments { get; set; } = new();
}