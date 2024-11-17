namespace ExcursionTickets.Core.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public string PaymentMethod { get; set; } 
        public decimal AmountPaid { get; set; }
        public decimal? Change { get; set; } 
        public int TicketQuantity { get; set; } 
        public int ExcursionId { get; set; }
        public Guid UserId { get; set; }
        public DateTime PaymentTime { get; set; }

        public Excursion Excursion { get; set; }
        public User User { get; set; }
        public ICollection<Ticket> Tickets { get; set; } 
    }
}
