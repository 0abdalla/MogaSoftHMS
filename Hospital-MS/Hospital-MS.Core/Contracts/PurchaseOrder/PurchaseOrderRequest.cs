using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PurchaseOrder;
public class PurchaseOrderRequest
{
    public DateTime Date { get; set; }
    public int CostCenterId { get; set; }
    public int SupplierId { get; set; }
    public int StoreId { get; set; }
    public string Currency { get; set; }
    public string Notes { get; set; }
    public List<PurchaseOrderItemRequest> Items { get; set; }
}
