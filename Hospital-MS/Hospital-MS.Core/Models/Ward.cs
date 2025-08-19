namespace Hospital_MS.Core.Models
{
    public sealed class Ward : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
    }
}
