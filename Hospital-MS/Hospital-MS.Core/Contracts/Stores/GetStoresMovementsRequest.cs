namespace Hospital_MS.Core.Contracts.Stores;
public class GetStoresMovementsRequest
{
    public DateOnly fromDate { get; set; }
    public DateOnly toDate { get; set; }

    public int? MainGroupId { get; set; }
    public int? ItemGroupId { get; set; }
}
