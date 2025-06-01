using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PurchaseOrder;
public class PurchaseOrderResponse
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public DateTime Date { get; set; }
    public string CostCenterName { get; set; }
    public string SupplierName { get; set; }
    public string StoreName { get; set; }
    public string Currency { get; set; }
    public string Notes { get; set; }
    public decimal TotalCost { get; set; }
    public List<PurchaseOrderItemResponse> Items { get; set; }
    public bool IsActive { get; set; }
    public AuditResponse Audit { get; set; }
}
