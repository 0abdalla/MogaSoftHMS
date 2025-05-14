using Hospital_MS.Core.Models;

public class DoctorMedicalService
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public int MedicalServiceId { get; set; }
    public MedicalService MedicalService { get; set; }
}
