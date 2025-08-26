using Hospital_MS.Core.Contracts.PriceQuotation;

namespace Hospital_MS.Core.Contracts.PurchaseRequests;
public class PurchaseRequestResponse
{
    public int Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public int StoreId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public List<PurchaseRequestItemResponse> Items { get; set; } = [];

    public PriceQuotationResponse? PriceQuotation { get; set; }
}
