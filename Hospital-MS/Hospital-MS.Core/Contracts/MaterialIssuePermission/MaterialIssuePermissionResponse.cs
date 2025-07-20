namespace Hospital_MS.Core.Contracts.MaterialIssuePermission;
public class MaterialIssuePermissionResponse
{
    public int Id { get; set; }
    public string PermissionNumber { get; set; }
    public string? DocumentNumber { get; set; }
    public DateOnly PermissionDate { get; set; }
    public int StoreId { get; set; }
    public string StoreName { get; set; }
    //public int BranchId { get; set; }
    //public string BranchName { get; set; }
    public int? JobDepartmentId { get; set; }
    public string? JobDepartmentName { get; set; }
    public string? Notes { get; set; }
    public List<MaterialIssueItemResponse> Items { get; set; } = new();

    public int? DisbursementRequestId { get; set; }
    public string? DisbursementRequestNumber { get; set; }
}