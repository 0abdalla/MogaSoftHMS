namespace Hospital_MS.Core.Contracts.Items;
public class GetItemMovementRequest
{
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public int StoreId { get; set; }
}
