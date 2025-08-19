namespace Hospital_MS.Core.Contracts.Rooms
{
    public class RoomResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public decimal DailyPrice { get; set; }

        public int WardId { get; set; }
        public string WardName { get; set; }
    }
}
