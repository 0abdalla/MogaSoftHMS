namespace Hospital_MS.Core.Contracts.Treasuries;
public class TreasuryMovementResponse
{
    public int Id { get; set; }
    public int TreasuryId { get; set; }
    public string TreasuryName { get; set; } = string.Empty;
    public int TreasuryNumber { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal ClosingBalance { get; set; }
    public DateOnly OpenedIn { get; set; }
    public DateOnly? ClosedIn { get; set; }
    public bool IsClosed { get; set; }
    public decimal TotalCredits { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal Balance { get; set; }
    public List<TransactionDetail> Transactions { get; set; } = new();
}
