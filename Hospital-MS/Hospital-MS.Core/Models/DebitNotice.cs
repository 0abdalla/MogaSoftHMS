namespace Hospital_MS.Core.Models;
public class DebitNotice : AuditableEntity // اشعار خصم
{
    public int Id { get; set; }
    public string DebitNoticeNumber { get; set; }
    public DateTime? Date { get; set; }
    public string? CheckNumber { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }


    public int AccountId { get; set; }
    public int BankId { get; set; }
    public int? DailyRestrictionId { get; set; }

    public Bank Bank { get; set; } = default!;
    public AccountTree Account { get; set; } = default!;
    public DailyRestriction? DailyRestriction { get; set; } = default!;
}
