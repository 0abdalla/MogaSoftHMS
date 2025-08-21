namespace Hospital_MS.Core.Contracts.Stores;
public class StoreRateResponse
{
    public int ItemGroupId { get; set; }
    public string ItemGroupName { get; set; }

    public List<StoreRateItemsResponse> Items { get; set; } = [];
}
