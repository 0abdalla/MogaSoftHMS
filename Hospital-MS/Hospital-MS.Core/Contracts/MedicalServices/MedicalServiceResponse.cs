namespace Hospital_MS.Core.Contracts.MedicalServices
{
    public class MedicalServiceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public List<MedicalServiceScheduleResponse> MedicalServiceSchedules { get; set; } = [];
        public List<RadiologyBodyTypeResponse> RadiologyBodyTypes { get; set; } = [];
    }
}
