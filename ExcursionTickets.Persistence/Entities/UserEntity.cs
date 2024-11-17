namespace ExcursionTickets.Persistence.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public ICollection<PaymentEntity> Payments { get; set; }
        public ICollection<TicketEntity> Tickets { get; set; }
    }
}
