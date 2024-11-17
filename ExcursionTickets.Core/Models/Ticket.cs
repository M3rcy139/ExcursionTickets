namespace ExcursionTickets.Core.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public int ExcursionId { get; set; }
        public Guid PaymentId { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }
        public Excursion Excursion { get; set; }
        public Payment Payment { get; set; }
    }
}
