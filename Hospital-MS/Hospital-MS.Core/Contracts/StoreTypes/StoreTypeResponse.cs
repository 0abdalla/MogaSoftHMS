namespace Hospital_MS.Core.Contracts.StoreTypes;

public class StoreTypeResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public bool? IsDeleted { get; set; }

}