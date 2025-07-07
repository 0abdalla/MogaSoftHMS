namespace Hospital_MS.Core.Contracts.RestrictionTypes;

public class RestrictionTypeResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}