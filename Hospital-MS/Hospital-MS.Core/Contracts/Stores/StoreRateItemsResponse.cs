namespace Hospital_MS.Core.Contracts.Stores;
public class StoreRateItemsResponse
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal Balance { get; set; } // رصيد الصنف الحالي
    public decimal TotalAmount { get; set; } // إجمالي المبلغ

}
