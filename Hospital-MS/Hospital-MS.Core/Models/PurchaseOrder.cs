using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models;
public class PurchaseOrder : AuditableEntity
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public string? Description { get; set; }
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;
    public bool IsActive { get; set; } = true;

    //public int? PriceQuotationId { get; set; }
    //public PriceQuotation? PriceQuotation { get; set; }

    public int? PurchaseRequestId { get; set; }
    public PurchaseRequest? PurchaseRequest { get; set; }
    public ICollection<PurchaseOrderItem> Items { get; set; } = new HashSet<PurchaseOrderItem>();
}