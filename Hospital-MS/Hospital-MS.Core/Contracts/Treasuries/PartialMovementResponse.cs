namespace Hospital_MS.Core.Contracts.Treasuries;
public class PartialMovementResponse
{
    public int MovementId { get; set; }
    public DateOnly OpenedIn { get; set; }
    public DateOnly? ClosedIn { get; set; }
}
