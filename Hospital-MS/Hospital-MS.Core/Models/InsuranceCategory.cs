namespace Hospital_MS.Core.Models
{
    public sealed class InsuranceCategory : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Rate { get; set; } = 0m;
        public bool IsActive { get; set; } = true;

        public int InsuranceCompanyId { get; set; }
        public InsuranceCompany InsuranceCompany { get; set; } = default!;
    }
}
