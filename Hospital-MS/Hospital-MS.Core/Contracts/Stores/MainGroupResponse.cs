namespace Hospital_MS.Core.Contracts.Stores;
public class MainGroupResponseV2
{
    public int MainGroupId { get; set; }
    public string MainGroupName { get; set; }
    public int? StoreId { get; set; }
    public string StoreName { get; set; }
    public List<ItemsOrderLimitResponseV2> ItemGroups { get; set; }
}

public class ItemsOrderLimitResponseV2
{
    public int ItemGroupId { get; set; }
    public string ItemGroupName { get; set; }
    public List<ItemLimitsResponseV2> Items { get; set; }
}

public class ItemLimitsResponseV2
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal OrderLimit { get; set; }
    public decimal Balance { get; set; }
}
