namespace Hospital_MS.Core.Contracts.PurchaseOrders;
public class PurchaseOrderRequest
{
    public string? ReferenceNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public int SupplierId { get; set; }
    public string? Description { get; set; }
    //public int? PriceQuotationId { get; set; }
    public int? PurchaseRequestId { get; set; }
    public List<PurchaseOrderItemRequest> Items { get; set; } = [];
}