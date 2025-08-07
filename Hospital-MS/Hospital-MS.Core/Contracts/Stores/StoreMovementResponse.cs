using Hospital_MS.Core.Contracts.ItemGroups;

namespace Hospital_MS.Core.Contracts.Stores;
public class StoreMovementResponse
{
    public int? MainGroupId { get; set; }
    public string? MainGroupName { get; set; }

    public List<ItemGroupsResponse> ItemGroups { get; set; } = [];

    public string? LastReceiptPermissionNumber { get; set; }
    public string? MaterialIssuePermissionNumber { get; set; }
}
