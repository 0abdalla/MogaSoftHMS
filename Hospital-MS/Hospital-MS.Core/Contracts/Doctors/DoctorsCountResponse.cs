namespace Hospital_MS.Core.Contracts.Doctors
{
    public class DoctorsCountResponse
    {
        public int TotalDoctors { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalActiveDoctors { get; set; } 
    }
}