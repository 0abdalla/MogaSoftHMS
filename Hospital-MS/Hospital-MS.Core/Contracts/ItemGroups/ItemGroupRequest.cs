namespace Hospital_MS.Core.Contracts.ItemGroups;

public class ItemGroupRequest
{
    public string Name { get; set; } = string.Empty;
    public int? MainGroupId { get; set; }
}