namespace Hospital_MS.Core.Contracts.Stores;
public class ItemsOrderLimitResponse
{
    //public int? StoreId { get; set; }
    //public string StoreName { get; set; }

    public int? ItemGroupId { get; set; }
    public string? ItemGroupName { get; set; }
    public int? StoreId { get; set; }
    public string? StoreName { get; set; }

    public int? MainGroupId { get; set; }
    public string? MainGroupName { get; set; }
    public List<ItemLimitsResponse> Items { get; set; } = [];
}
