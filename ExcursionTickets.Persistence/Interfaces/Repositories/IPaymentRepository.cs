using ExcursionTickets.Core.Models;

namespace ExcursionTickets.Persistence.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> ProcessPayment(User user, decimal amountPaid, string paymentMethod, int ticketQuantity, int excursionId);
        Task<List<Ticket>> GetTickets(Guid paymentId);
    }
}
