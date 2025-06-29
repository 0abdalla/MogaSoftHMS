using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PurchasePermission;
public class ReceiptPermissionResponse
{
    public int Id { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateOnly PermissionDate { get; set; }
    public string? Notes { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public int StoreId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<ReceiptPermissionItemResponse> Items { get; set; } = new();
}
