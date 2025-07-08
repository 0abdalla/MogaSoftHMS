using Hospital_MS.Core.Contracts.Common;

namespace Hospital_MS.Core.Contracts.ItemGroups;

public class ItemGroupResponse
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public int? MainGroupId { get; set; }
    public string? MainGroupName { get; set; }

    public AuditResponse Audit { get; set; } = new();
}