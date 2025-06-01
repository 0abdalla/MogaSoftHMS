using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PurchaseOrder;
public class PurchaseOrderItemRequest
{
    public int ItemId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
}