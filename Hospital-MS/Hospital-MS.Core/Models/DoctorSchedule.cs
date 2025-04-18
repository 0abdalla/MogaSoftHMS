namespace Hospital_MS.Core.Models
{
    public sealed class DoctorSchedule : AuditableEntity
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int WeekDay { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }


        public Staff Doctor { get; set; } = default!;
    }
}
