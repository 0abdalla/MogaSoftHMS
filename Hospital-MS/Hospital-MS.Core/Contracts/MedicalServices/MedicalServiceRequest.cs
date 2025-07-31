namespace Hospital_MS.Core.Contracts.MedicalServices
{
    public class MedicalServiceRequest
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Type { get; set; }
        public string? RadiologyBodyTypeName { get; set; }
        public List<string> WeekDays { get; set; } = [];
    }

    public class RadiologyBodyTypeRequest
    {
        public int MedicalServiceId { get; set; }
        public string Name { get; set; }
    }


}
