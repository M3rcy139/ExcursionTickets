using ExcursionTickets.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExcursionTickets.Persistence.Configurations
{
    public class ExcursionConfiguration : IEntityTypeConfiguration<ExcursionEntity>
    {
        public void Configure(EntityTypeBuilder<ExcursionEntity> builder)
        {
            builder.ToTable("Excursions");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(40);

            builder.Property(e => e.Description)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(e => e.AvailableTickets)
                   .IsRequired();

            builder.Property(e => e.Price)
                   .HasColumnType("decimal(18,2)");

            builder.Property(t => t.StartTime)
                   .IsRequired();
        }
    }
}
