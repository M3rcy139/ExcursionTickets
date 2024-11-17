using AutoMapper;
using ExcursionTickets.Persistence.Entities;
using ExcursionTickets.Core.Models;

namespace ExcursionTickets.Persistence.Mappings
{
    public class DataBaseMappings : Profile
    {
        public DataBaseMappings()
        {
            CreateMap<AdvertisementEntity, Advertisement>();
            CreateMap<ExcursionEntity, Excursion>();
            CreateMap<PaymentEntity, Payment>();
            CreateMap<TicketEntity, Ticket>();
            CreateMap<UserEntity, User>();
        }
    }
}
