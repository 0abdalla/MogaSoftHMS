using Hospital_MS.Core.Contracts.Common;

namespace Hospital_MS.Core.Contracts.AccountingGuidance;

public class AccountingGuidanceResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public AuditResponse Audit { get; set; } = new();
}