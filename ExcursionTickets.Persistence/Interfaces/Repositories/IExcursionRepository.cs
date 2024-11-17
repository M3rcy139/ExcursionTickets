using ExcursionTickets.Core.Models;

namespace ExcursionTickets.Persistence.Interfaces.Repositories
{
    public interface IExcursionRepository
    {
        Task<List<Excursion>> GetAllExcursions();
        Task<Excursion> GetExcursionDetails(int excursionId);
        Task UpdateExcursion(int excursionId, int ticketQuantity);
    }
}
