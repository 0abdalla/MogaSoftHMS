
public class Supplier
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string AccountCode { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string Address { get; set; }

    [Required]
    [MaxLength(100)]
    public string ResponsibleName1 { get; set; }

    [MaxLength(100)]
    public string? ResponsibleName2 { get; set; }

    [Required]
    [MaxLength(15)]
    public string Phone1 { get; set; }

    [MaxLength(15)]
    public string? Phone2 { get; set; }

    [Required]
    [MaxLength(50)]
    public string TaxNumber { get; set; }

    [MaxLength(100)]
    public string? Job { get; set; }

    [MaxLength(20)]
    public string? Fax1 { get; set; }

    [MaxLength(20)]
    public string? Fax2 { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [MaxLength(200)]
    public string? Website { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}