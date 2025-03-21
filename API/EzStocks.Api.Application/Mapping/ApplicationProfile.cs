using AutoMapper;

namespace EzStocks.Api.Application.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Domain.Entities.StockTicker, Dtos.StockTicker>();
            CreateMap<Dtos.StockTicker, Domain.Entities.StockTicker>();
            CreateMap<Dtos.StockPriceItem, Domain.Entities.StockPriceItem>();
            CreateMap<Dtos.User, Domain.Entities.User>()
                .ForMember(d => d.StockItems, o => o.Ignore());
            CreateMap<Domain.Entities.StockTicker, Domain.Entities.UserStockTicker>();
            CreateMap<Domain.Entities.User, Dtos.User>();
            CreateMap<Domain.Entities.UserStockTicker, Dtos.UserStockItem>();
            CreateMap<Services.TickerSymbol, Dtos.TickerSymbol>();
        }
    }
}
