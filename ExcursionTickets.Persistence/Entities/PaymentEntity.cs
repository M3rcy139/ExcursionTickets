namespace ExcursionTickets.Persistence.Entities
{
    public class PaymentEntity
    {
        public Guid Id { get; set; }
        public string PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal? Change { get; set; }
        public int TicketQuantity { get; set; }
        public int ExcursionId { get; set; }
        public Guid UserId { get; set; }
        public DateTime PaymentTime { get; set; }

        public ExcursionEntity Excursion { get; set; }
        public UserEntity User { get; set; }
        public ICollection<TicketEntity> Tickets { get; set; }
    }
}
