namespace Hospital_MS.Core.Contracts.Stores
{
    public class StoreResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public int? StoreTypeId { get; set; }
        public string? StoreTypeName { get; set; } 
        public bool IsActive { get; set; }
    }
}