using Hospital_MS.Core.Contracts.Common;

namespace Hospital_MS.Core.Contracts.Treasuries;
public class TreasuryResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; }
    public string Currency { get; set; }
    public bool IsActive { get; set; }
    public AuditResponse Audit { get; set; }
    public List<PartialMovementResponse> Movements { get; set; } = [];
}
