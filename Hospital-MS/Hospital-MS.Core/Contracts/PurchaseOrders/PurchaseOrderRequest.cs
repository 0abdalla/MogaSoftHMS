using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PurchaseOrders;
public class PurchaseOrderRequest
{
    public string? ReferenceNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public int SupplierId { get; set; }
    public string? Description { get; set; }
    public List<PurchaseOrderItemRequest> Items { get; set; } = [];
}