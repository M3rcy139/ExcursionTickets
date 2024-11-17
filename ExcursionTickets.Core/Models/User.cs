namespace ExcursionTickets.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
