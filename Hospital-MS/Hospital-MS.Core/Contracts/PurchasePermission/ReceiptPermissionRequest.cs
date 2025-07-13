using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PurchasePermission;
public class ReceiptPermissionRequest
{
    //public string PermissionNumber { get; set; }
    public string DocumentNumber { get; set; }
    public DateTime PermissionDate { get; set; }
    public string Notes { get; set; }
    public List<ReceiptPermissionItemRequest> Items { get; set; } = new List<ReceiptPermissionItemRequest>();
    public int StoreId { get; set; }
    public int SupplierId { get; set; }
    public int PurchaseOrderId { get; set; }
}
