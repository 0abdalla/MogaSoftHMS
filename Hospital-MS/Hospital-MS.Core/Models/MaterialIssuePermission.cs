using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models.HR;

namespace Hospital_MS.Core.Models;
public class MaterialIssuePermission : AuditableEntity // اذن الصرف
{
    public int Id { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public DateOnly PermissionDate { get; set; }
    public string? DocumentNumber { get; set; }
    public int StoreId { get; set; }
    public Store Store { get; set; }
    //public int BranchId { get; set; }
    // public Branch Branch { get; set; }
    public string? Notes { get; set; }

    public int? JobDepartmentId { get; set; }
    public JobDepartment? JobDepartment { get; set; } = default!;

    public int? DisbursementRequestId { get; set; }
    public DisbursementRequest? DisbursementRequest { get; set; } = default!;
    public ICollection<MaterialIssueItem> Items { get; set; } = new List<MaterialIssueItem>();
    public MaterialIssueType Type { get; set; }

    public DailyRestriction DailyRestriction { get; set; } = default!;
    public int? DailyRestrictionId { get; set; }
}