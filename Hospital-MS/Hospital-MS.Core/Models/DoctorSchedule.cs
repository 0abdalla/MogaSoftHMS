namespace Hospital_MS.Core.Models
{
    public sealed class DoctorSchedule : AuditableEntity
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string WeekDay { get; set; } = string.Empty; 
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }


        
        //public Staff Doctor { get; set; } = default!;
        public Doctor Doctor { get; set; } = default!;
    }
}
