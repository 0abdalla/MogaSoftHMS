namespace Hospital_MS.Core.Models
{
    public class MedicalService : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal? Price { get; set; } = 0M;
        public string Type { get; set; } = string.Empty;

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public ICollection<DoctorMedicalService> DoctorMedicalServices { get; set; } = new HashSet<DoctorMedicalService>();
        public ICollection<MedicalServiceSchedule> Schedules { get; set; } = new HashSet<MedicalServiceSchedule>();
    }
}
