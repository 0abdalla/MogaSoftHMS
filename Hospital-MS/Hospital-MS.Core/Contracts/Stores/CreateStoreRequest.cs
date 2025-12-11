namespace Hospital_MS.Core.Contracts.Stores
{
    public class CreateStoreRequest
    {
        public string Name { get; set; } = string.Empty;
        public int AccountId { get; set; }
        public int StoreTypeId { get; set; }
    }
}