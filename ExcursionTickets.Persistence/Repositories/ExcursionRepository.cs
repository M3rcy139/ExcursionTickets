using AutoMapper;
using ExcursionTickets.Core.Models;
using ExcursionTickets.Persistence.Entities;
using ExcursionTickets.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExcursionTickets.Persistence.Repositories
{
    public class ExcursionRepository : IExcursionRepository
    {
        private readonly ExcursionDbContext _context;
        private IMapper _mapper;
        public ExcursionRepository(ExcursionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Excursion>> GetAllExcursions()
        {
            var excursions = await _context.Excursions
                .Select(a => a)
                .ToListAsync();

            if (!excursions.Any())
                throw new ArgumentException("Ни одной экскурсии не найдено");

            return _mapper.Map<List<Excursion>>(excursions);
        }

        public async Task<Excursion> GetExcursionDetails(int excursionId)
        {
            var excursions = await _context.Excursions
               .FirstOrDefaultAsync(e => e.Id == excursionId) ?? throw new ArgumentException("Данная экскурсия не существует");

            return _mapper.Map<Excursion>(excursions);
        }

        public async Task UpdateExcursion(int excursionId, int ticketQuantity)
        {
            var excursion = await _context.Excursions
                .FirstOrDefaultAsync(e => e.Id == excursionId);

            excursion.AvailableTickets -= ticketQuantity;

            await _context.SaveChangesAsync();
        }
    }
}
