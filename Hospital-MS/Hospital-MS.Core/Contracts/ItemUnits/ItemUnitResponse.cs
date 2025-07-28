using Hospital_MS.Core.Contracts.Common;

namespace Hospital_MS.Core.Contracts.ItemUnits;
public class ItemUnitResponse
{
    public int Id { get; set; }
    public string Name { get; set; }

    public AuditResponse Audit { get; set; } = new();
}
