using AutoMapper;
using PaymentGateway.Read.Models;
using PaymentGateway.Read.Repositories.Entities;

namespace PaymentGateway.Read.Repositories.AutoMapper
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<PaymentView, PaymentEntity>()
               .ForMember(t => t.PartitionKey, cfg => cfg.MapFrom(view => view.Id))
               .ForMember(t => t.RowKey, cfg => cfg.MapFrom(view => view.Id));

            CreateMap<PaymentEntity, PaymentView>()
               .ForMember(t => t.Id, cfg => cfg.MapFrom(view => view.PartitionKey));
        }
    }
}
