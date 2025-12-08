namespace Hospital_MS.Core.Contracts.Wards
{
    public class WardResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool? IsDeleted { get; set; }

    }
}
