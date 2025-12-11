using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models;
public class ReceiptPermission : AuditableEntity
{
    public int Id { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateOnly PermissionDate { get; set; }
    public int StoreId { get; set; }
    public int? TargetId { get; set; }
    public ReceiptType Type { get; set; }

    public int PurchaseOrderId { get; set; }
    public string? Notes { get; set; }

    public Supplier Supplier { get; set; } = default!;
    public Store Store { get; set; } = default!;
    public PurchaseOrder PurchaseOrder { get; set; } = default!;
    public ICollection<ReceiptPermissionItem> Items { get; set; } = new HashSet<ReceiptPermissionItem>();

    public DailyRestriction DailyRestriction { get; set; } = default!;
    public int? DailyRestrictionId { get; set; }
    public bool IsPaid { get; set; } = false;

    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public List<ReceiptPayment> Payments { get; set; } = [];
}

public class ReceiptPayment : AuditableEntity
{
    public int Id { get; set; }
    public int ReceiptPermissionId { get; set; }
    public ReceiptPermission ReceiptPermission { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentType PaymentType { get; set; }
    public string? DocumentNumber { get; set; }
}
