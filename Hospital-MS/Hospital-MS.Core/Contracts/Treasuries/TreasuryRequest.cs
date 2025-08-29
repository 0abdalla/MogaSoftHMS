namespace Hospital_MS.Core.Contracts.Treasuries;
public class TreasuryRequest
{
    public string Code { get; set; }
    public string Name { get; set; }
    public int BranchId { get; set; }
    public string Currency { get; set; }
    public decimal OpeningBalance { get; set; }
}
