namespace Hospital_MS.Core.Contracts.Branches;

public class BranchRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
}