namespace Hospital_MS.Core.Contracts.Items;
public class ReceiptItemsResponse
{
    public string ReceiptNumber { get; set; }
    public string? DocumentNumber { get; set; }
    public DateOnly Date { get; set; }
    public string? ReceiptType { get; set; }
    public decimal TotalReceiptsQuantity { get; set; } = 0M;
    public decimal TotalPrice { get; set; } = 0M;
    public string? SupplierName { get; set; }

    public decimal UnitPrice { get; set; }
}
