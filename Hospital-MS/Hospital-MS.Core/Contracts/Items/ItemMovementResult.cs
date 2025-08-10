namespace Hospital_MS.Core.Contracts.Items;
public class ItemMovementResult
{
    public int ItemId { get; set; }
    public string? ItemName { get; set; }
    public string? ItemGroupName { get; set; }
    public string? ItemUnit { get; set; }
    public string? StoreName { get; set; }
    public List<ReceiptItemsResponse> ItemReceipts { get; set; } = [];
    public List<IssueItemsResponse> ItemIssues { get; set; } = [];

    public decimal PreviousBalance { get; set; }
    public decimal TotalItemIssues { get; set; }
    public decimal TotalItemReceipts { get; set; }
}
