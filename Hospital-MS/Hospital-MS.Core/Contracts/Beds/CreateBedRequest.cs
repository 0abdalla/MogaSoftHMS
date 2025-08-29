namespace Hospital_MS.Core.Contracts.Beds
{
    public class CreateBedRequest
    {
        public int Number { get; set; }
        public string Status { get; set; }
        public decimal DailyPrice { get; set; }
        public int RoomId { get; set; }
    }
}
