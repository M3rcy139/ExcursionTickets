using AutoMapper;
using ExcursionTickets.Core.Models;
using ExcursionTickets.Persistence.Entities;
using ExcursionTickets.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExcursionTickets.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ExcursionDbContext _context;
        private IMapper _mapper;
        private IExcursionRepository _excursionRepository;
        public PaymentRepository(ExcursionDbContext context, IMapper mapper, IExcursionRepository excursionRepository)
        {
            _context = context;
            _mapper = mapper;
            _excursionRepository = excursionRepository;
        }

        public async Task<Payment> ProcessPayment(User user, decimal amountPaid, string paymentMethod, int ticketQuantity, int excursionId)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
            };

            var excursion = await _excursionRepository.GetExcursionDetails(excursionId);

            decimal totalCost = excursion.Price * ticketQuantity;

            if (totalCost > amountPaid)
                throw new InvalidOperationException("Недостаточно средств");

            decimal changeGiven = amountPaid - totalCost;

            var ticketEntities = new List<TicketEntity>();

            var paymentEntity = new PaymentEntity
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                ExcursionId = excursionId,
                AmountPaid = amountPaid,
                PaymentMethod = paymentMethod,
                PaymentTime = DateTime.UtcNow,
                Change = changeGiven,
                TicketQuantity = ticketQuantity
            };

            await _excursionRepository.UpdateExcursion(excursionId, ticketQuantity);

            while (ticketQuantity > 0)
            {
                var ticketEntity = await GetInfoForTicket(user, excursionId, paymentEntity.Id);

                ticketEntities.Add(ticketEntity);

                ticketQuantity -= 1;
            }

            await _context.Users.AddAsync(userEntity);
            await _context.Payments.AddAsync(paymentEntity);
            await _context.Tickets.AddRangeAsync(ticketEntities);
            await _context.SaveChangesAsync();

            return _mapper.Map<Payment>(paymentEntity);
        }

        private async Task<TicketEntity> GetInfoForTicket(User user, int excursionId, Guid paymentId)
        {
            var seat = await _context.Excursions
                .FirstOrDefaultAsync(e => e.Id == excursionId)
                ?? throw new InvalidOperationException($"Данное место ({excursionId}) не существует");

            var ticketEntity = new TicketEntity
            {
                Id = Guid.NewGuid(),
                ExcursionId = excursionId,
                PaymentId = paymentId,
                UserId = user.Id
            };

            return ticketEntity;
        }

        public async Task<List<Ticket>> GetTickets(Guid paymentId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Excursion)
                .Include(t => t.User)
                .Include(t => t.Payment)
                .Where(t => t.PaymentId == paymentId)
                .ToListAsync()
                ?? throw new ArgumentException("Билет найти не удалось");

            return _mapper.Map<List<Ticket>>(tickets);
        }
    }
}
