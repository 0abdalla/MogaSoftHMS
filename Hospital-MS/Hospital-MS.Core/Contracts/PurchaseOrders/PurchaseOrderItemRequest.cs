using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PurchaseOrders;
public class PurchaseOrderItemRequest
{
    public int ItemId { get; set; }
    public string? Unit { get; set; }
    public decimal RequestedQuantity { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? TotalPrice { get; set; }

}