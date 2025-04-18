namespace Hospital_MS.Core.Models
{
    public sealed class NurseShift : AuditableEntity
    {
        public int Id { get; set; }
        public int NurseId { get; set; }
        public DateOnly ShiftDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        //public string Ward { get; set; } = string.Empty;


        public int WardId { get; set; }
        public Ward Ward { get; set; } =default!;
        public Staff Nurse { get; set; } = default!;
    }
}
