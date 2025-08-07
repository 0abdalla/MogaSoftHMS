namespace Hospital_MS.Core.Contracts.Items;
public class ItemMovementResponse
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal OpeningBalance { get; set; }
    public decimal ReceivedBalance { get; set; }
    public decimal IssueBalance { get; set; }
    public decimal TotalBalance { get; set; }
}
