namespace Hospital_MS.Core.Models;
public class ReceiptPermissionItem : AuditableEntity
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; } = default!;
    public string Unit { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;

    public ReceiptPermission? ReceiptPermission { get; set; } = default!;
    public int? ReceiptPermissionId { get; set; }
}