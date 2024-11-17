using ExcursionTickets.Core.Models;

namespace ExcursionTickets.Persistence.Interfaces.Repositories
{
    public interface IAdvertisementRepository
    {
        Task<List<Advertisement>> GetAdvertisements();
    }
}
