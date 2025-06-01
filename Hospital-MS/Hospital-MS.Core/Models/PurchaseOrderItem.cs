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
    public int ItemId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public PurchaseOrder PurchaseOrder { get; set; }
    public Item Item { get; set; }
}