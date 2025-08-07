namespace Hospital_MS.Core.Contracts.Items;
public class IssueItemsResponse
{
    public int IssueId { get; set; }
    public string? DocumentNumber { get; set; }
    public DateOnly Date { get; set; }
    public string? IssueType { get; set; }
    public string? InvoiceNumber { get; set; }
    public decimal TotalIssuesQuantity { get; set; } = 0M;
    public decimal TotalPrice { get; set; } = 0M;
    public string? SupplierName { get; set; }

    public decimal UnitPrice { get; set; }
}
