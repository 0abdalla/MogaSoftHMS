using Hospital_MS.Core.Contracts.DailyRestrictions;

namespace Hospital_MS.Core.Contracts.MaterialIssuePermission;
public class MaterialIssuePermissionToReturnResponse
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string JobDepartmentName { get; set; }
    public string StoreName { get; set; }

    public PartialDailyRestrictionResponse DailyRestriction { get; set; } = new();
}
