namespace ExcursionTickets.Persistence.Entities
{
    public class TicketEntity
    {
        public Guid Id { get; set; }
        public int ExcursionId { get; set; }
        public Guid PaymentId { get; set; }
        public Guid UserId { get; set; }

        public UserEntity User { get; set; }
        public ExcursionEntity Excursion { get; set; }
        public PaymentEntity Payment { get; set; }
    }
}
