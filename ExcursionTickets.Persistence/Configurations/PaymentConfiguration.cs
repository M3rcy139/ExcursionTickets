using ExcursionTickets.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ExcursionTickets.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<PaymentEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentEntity> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.PaymentMethod)
                   .IsRequired()
                   .HasMaxLength(30);

            builder.Property(p => p.AmountPaid)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.Change)
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentTime)
                   .IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(p => p.Tickets)
                   .WithOne(t => t.Payment)
                   .HasForeignKey(t => t.PaymentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
