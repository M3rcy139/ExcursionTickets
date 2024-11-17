using ExcursionTickets.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ExcursionTickets.Persistence.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<TicketEntity>
    {
        public void Configure(EntityTypeBuilder<TicketEntity> builder)
        {
            builder.ToTable("Tickets");

            builder.HasKey(t => t.Id);

            builder.HasOne(e => e.Excursion)
                   .WithMany()
                   .HasForeignKey(t => t.ExcursionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Payment)
                   .WithMany(p => p.Tickets)
                   .HasForeignKey(t => t.PaymentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.User)
                   .WithMany()
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
