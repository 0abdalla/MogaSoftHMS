namespace Hospital_MS.Core.Contracts.Stores
{
    public class CreateStoreRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int StoreTypeId { get; set; }
    }
}