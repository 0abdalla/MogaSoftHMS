namespace Hospital_MS.Core.Contracts.RestrictionTypes;

public class RestrictionTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}