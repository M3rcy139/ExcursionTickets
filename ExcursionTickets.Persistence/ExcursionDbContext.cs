using ExcursionTickets.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExcursionTickets.Persistence
{
    public class ExcursionDbContext(DbContextOptions<ExcursionDbContext> options) : DbContext(options)
    {
        public DbSet<AdvertisementEntity> Advertisements { get; set; }
        public DbSet<ExcursionEntity> Excursions { get; set; }
        public DbSet<TicketEntity> Tickets { get; set; }
        public DbSet<PaymentEntity> Payments { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExcursionDbContext).Assembly);
        }
    }
}
