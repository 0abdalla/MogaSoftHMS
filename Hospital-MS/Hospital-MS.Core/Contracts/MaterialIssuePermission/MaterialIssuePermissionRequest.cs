namespace Hospital_MS.Core.Contracts.MaterialIssuePermission;
public class MaterialIssuePermissionRequest
{
    // public string? DocumentNumber { get; set; }
    public DateTime PermissionDate { get; set; }
    public int StoreId { get; set; }
    public int? JobDepartmentId { get; set; }
    public string? Notes { get; set; }
    public int? DisbursementRequestId { get; set; }
    public List<MaterialIssueItemRequest> Items { get; set; } = [];
}
