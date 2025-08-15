namespace Hospital_MS.Core.Contracts.PurchasePermission;
public class ReceiptPermissionRequest
{
    //public string PermissionNumber { get; set; }
    public string DocumentNumber { get; set; }
    public DateOnly PermissionDate { get; set; }
    public string Notes { get; set; }
    public List<ReceiptPermissionItemRequest> Items { get; set; } = new List<ReceiptPermissionItemRequest>();
    public int StoreId { get; set; }
    public int SupplierId { get; set; }
    public int PurchaseOrderId { get; set; }
}
