using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class ReceiptPermission : AuditableEntity 
{
    public int Id { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateOnly PermissionDate { get; set; }
    public int StoreId { get; set; }
    public int SupplierId { get; set; }
    //public int PurchaseRequestId { get; set; }
    public int PurchaseOrderId { get; set; }
    public PurchasePermissionStatus Status { get; set; } = PurchasePermissionStatus.Active;
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;

    public Supplier Supplier { get; set; } = default!;
    public Store Store { get; set; } = default!;
    //public PurchaseRequest PurchaseRequest { get; set; } = default!;
    public PurchaseOrder PurchaseOrder { get; set; } = default!;
    public ICollection<ReceiptPermissionItem> Items { get; set; } = new HashSet<ReceiptPermissionItem>();
}
