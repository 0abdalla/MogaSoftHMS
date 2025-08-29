namespace Hospital_MS.Core.Contracts.Stores;
public class StoreRateResponse
{
    public int ItemGroupId { get; set; }
    public string ItemGroupName { get; set; }

    public List<StoreRateItemsResponse> Items { get; set; } = [];
}

public class StoreRateResponseV2
{
    public int MainGroupId { get; set; }
    public string MainGroupName { get; set; }
    public int StoreId { get; set; }
    public string StoreName { get; set; }
    public List<StoreRateItemGroupResponseV2> ItemGroups { get; set; }
}

public class StoreRateItemGroupResponseV2
{
    public int ItemGroupId { get; set; }
    public string ItemGroupName { get; set; }
    public List<StoreRateItemsResponseV2> Items { get; set; }
}

public class StoreRateItemsResponseV2
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal Balance { get; set; }
    public decimal TotalAmount { get; set; }
}
