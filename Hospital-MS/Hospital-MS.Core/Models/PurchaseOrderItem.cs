using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class PurchaseOrderItem : AuditableEntity
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; } = null!;
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public string? Unit { get; set; }
    public decimal RequestedQuantity { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total => Quantity * UnitPrice;

    public decimal? TotalPrice { get; set; }
    public bool IsActive { get; set; } = true;
}