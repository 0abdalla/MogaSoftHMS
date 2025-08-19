namespace Hospital_MS.Core.Contracts.Stores;
public class ItemLimitsResponse
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal OrderLimit { get; set; }
    public decimal Balance { get; set; } // رصيد الصنف الحالي
}
