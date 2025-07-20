namespace Hospital_MS.Core.Contracts.Treasuries;
public class TreasuryTransactionRow
{
    public int DocumentNumber { get; set; }
    public DateOnly Date { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
}
