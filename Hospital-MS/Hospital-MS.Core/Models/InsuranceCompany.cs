namespace Hospital_MS.Core.Models
{
    public sealed class InsuranceCompany : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; } 
        public string Phone { get; set; } = string.Empty;
        public string? Code { get; set; } 
        public DateOnly ContractStartDate { get; set; }
        public DateOnly ContractEndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Patient> Patients { get; set; } = new HashSet<Patient>();
        public ICollection<InsuranceCategory> Categories { get; set; } = new HashSet<InsuranceCategory>();
    }
}
