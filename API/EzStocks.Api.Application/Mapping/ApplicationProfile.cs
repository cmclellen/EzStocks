using AutoMapper;

namespace EzStocks.Api.Application.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Domain.Entities.StockItem, Dtos.StockItem>();
        }
    }
}
