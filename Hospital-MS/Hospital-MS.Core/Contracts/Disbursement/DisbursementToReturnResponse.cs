namespace Hospital_MS.Core.Contracts.Disbursement;
public class DisbursementToReturnResponse
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string? DepartmentName { get; set; }
    public List<string> ItemsNames { get; set; } = [];
}
