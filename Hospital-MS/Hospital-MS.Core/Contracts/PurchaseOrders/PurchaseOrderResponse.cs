namespace Hospital_MS.Core.Contracts.PurchaseOrders;
public class PurchaseOrderResponse
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public DateTime OrderDate { get; set; }
    //public int? PriceQuotationId { get; set; }
    //public string? PriceQuotationNumber { get; set; }

    public int? PurchaseRequestId { get; set; }
    public string? PurchaseRequestNumber { get; set; }

    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<PurchaseOrderItemResponse> Items { get; set; } = [];
}