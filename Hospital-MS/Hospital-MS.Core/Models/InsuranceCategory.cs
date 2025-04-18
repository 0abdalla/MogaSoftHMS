namespace Hospital_MS.Core.Models
{
    public sealed class InsuranceCategory : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public ICollection<Patient> Patients { get; set; } = new HashSet<Patient>();
    }
}
