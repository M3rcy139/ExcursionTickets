namespace ExcursionTickets.Persistence.Entities
{
    public class ExcursionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AvailableTickets { get; set; }
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
    }
}
