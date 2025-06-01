namespace Hospital_MS.Core.Models;
public class Customer : AuditableEntity
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? ResponsibleName { get; set; }
    public string? ResponsibleName2 { get; set; }
    public string? CustomerType { get; set; }
    public string Job { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Phone2 { get; set; }
    public string? Telephone { get; set; }
    public string? Telephone2 { get; set; }
    public string? Email { get; set; }
    public string? Notes { get; set; }

    public string? PaymentMethod { get; set; }
    public string? PaymentResponsible { get; set; }
    public decimal CreditLimit { get; set; }
    public bool IsActive { get; set; } = true;
}
