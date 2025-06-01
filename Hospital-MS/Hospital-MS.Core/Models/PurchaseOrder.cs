using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class PurchaseOrder : AuditableEntity
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CostCenterId { get; set; }
    public int SupplierId { get; set; }
    public int StoreId { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public decimal TotalCost { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public CostCenter CostCenter { get; set; } = default!;
    public Supplier Supplier { get; set; } = default!;
    public Store Store { get; set; } = default!;
    public ICollection<PurchaseOrderItem> Items { get; set; } = new HashSet<PurchaseOrderItem>();
}