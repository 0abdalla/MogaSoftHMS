namespace Hospital_MS.Core.Contracts.DailyRestrictions;
public class AccountReportResponse
{
    public int DailyRestrictionId { get; set; }
    public string? DailyRestrictionNumber { get; set; }
    public DateOnly DailyRestrictionDate { get; set; }
    public int? AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? Description { get; set; }
    public decimal Debits { get; set; }
    public decimal Credits { get; set; }
    public decimal Balance { get; set; }

    public string? From { get; set; }
    public string? To { get; set; }
}
