using ExcursionTickets.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ExcursionTickets.Persistence.Configurations
{
    public class AdvertisementConfiguration : IEntityTypeConfiguration<AdvertisementEntity>
    {
        public void Configure(EntityTypeBuilder<AdvertisementEntity> builder)
        {
            builder.ToTable("Advertisement");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.AdText)
                   .IsRequired()
                   .HasMaxLength(300);
        }
    }
}
