using AutoMapper;
using ExcursionTickets.Core.Models;
using ExcursionTickets.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExcursionTickets.Persistence.Repositories
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly ExcursionDbContext _context;
        private IMapper _mapper;
        public AdvertisementRepository(ExcursionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Advertisement>> GetAdvertisements()
        {
            var advertisements = await _context.Advertisements
                .Select(a => a)
                .ToListAsync();

            if (!advertisements.Any())
                throw new ArgumentException("Ни одной рекламы не найдено");

            return _mapper.Map<List<Advertisement>>(advertisements);
        }
    }
}
