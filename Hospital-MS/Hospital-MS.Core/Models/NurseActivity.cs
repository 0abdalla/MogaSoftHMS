namespace Hospital_MS.Core.Models
{
    public sealed class NurseActivity : AuditableEntity
    {
        public int Id { get; set; }
        public int NurseId { get; set; }
        public int PatientId { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ActivityDescription { get; set; } = string.Empty;

        
        public Staff Nurse { get; set; } = default!;
        public Patient Patient { get; set; } = default!;
    }
}
