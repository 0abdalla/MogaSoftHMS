namespace Hospital_MS.Core.Contracts.Treasuries;
public class PartialMovementResponse
{
    public int MovementId { get; set; }
    public DateOnly OpenedIn { get; set; }
    public DateOnly? ClosedIn { get; set; }
    public bool IsClosed { get; set; }
    public int TreasuryNumber { get; set; }
    public bool IsReEnabled { get; set; }
}
