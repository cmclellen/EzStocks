using AutoMapper;

namespace EzStocks.Api.Application.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Domain.Entities.StockItem, Dtos.StockItem>();
            CreateMap<Dtos.StockItem, Domain.Entities.StockItem>();
            CreateMap<Dtos.StockPriceItem, Domain.Entities.StockPriceItem>();
            CreateMap<Dtos.User, Domain.Entities.User>()
                .ForMember(d => d.StockItems, o => o.Ignore());
            CreateMap<Domain.Entities.StockItem, Domain.Entities.UserStockItem>();
            CreateMap<Domain.Entities.User, Dtos.User>();
            CreateMap<Domain.Entities.UserStockItem, Dtos.UserStockItem>();
            CreateMap<Services.TickerSymbol, Dtos.TickerSymbol>();
        }
    }
}
