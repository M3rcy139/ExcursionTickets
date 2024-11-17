using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ExcursionTickets.Persistence.Interfaces.Repositories;
using ExcursionTickets.Persistence.Repositories;

namespace ExcursionTickets.Persistence
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services
                , IConfiguration configuration)
        {
            services.AddDbContext<ExcursionDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(ExcursionDbContext)));
            });

            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
            services.AddScoped<IExcursionRepository, ExcursionRepository>();

            return services;
        }
    }
}
