using Hospital_MS.Core.Contracts.Items;

namespace Hospital_MS.Core.Contracts.ItemGroups;
public class ItemGroupsResponse
{
    public int? ItemGroupId { get; set; }
    public string? ItemGroupName { get; set; }

    public List<ItemMovementResponse> Items { get; set; } = [];
}
