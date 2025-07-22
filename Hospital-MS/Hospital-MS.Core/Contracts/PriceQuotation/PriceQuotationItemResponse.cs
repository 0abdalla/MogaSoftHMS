namespace Hospital_MS.Core.Contracts.PriceQuotation;
public class PriceQuotationItemResponse
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Unit { get; set; }
    public decimal Total { get; set; }
    public string? Notes { get; set; }
}
