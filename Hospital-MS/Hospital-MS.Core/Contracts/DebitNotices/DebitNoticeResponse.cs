using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.DailyRestrictions;

namespace Hospital_MS.Core.Contracts.DebitNotices;
public class DebitNoticeResponse
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int BankId { get; set; }
    public string BankName { get; set; }
    public int AccountId { get; set; }
    public string AccountName { get; set; }
    public string? CheckNumber { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public AuditResponse Audit { get; set; } = new();

    public PartialDailyRestrictionResponse DailyRestriction { get; set; } = new();
}