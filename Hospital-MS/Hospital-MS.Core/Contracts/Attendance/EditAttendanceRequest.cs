namespace Hospital_MS.Core.Contracts.Attendance
{
    public class EditAttendanceRequest
    {
        public TimeOnly? InTime { get; set; }
        public TimeOnly? OutTime { get; set; }
        public string? Notes { get; set; }
    }
}