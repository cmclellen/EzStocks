using AutoMapper;
using EzStocks.Api.Application.Commands;

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
                .ForMember(d => d.StockTickers, o => o.Ignore());
            CreateMap<Domain.Entities.StockTicker, Domain.Entities.UserStockTicker>();
            CreateMap<Domain.Entities.User, Dtos.User>();
            CreateMap<Domain.Entities.StockTicker, Dtos.StockTicker>();
            CreateMap<Services.StockTicker, Dtos.StockTickerSmall>();
            CreateMap<Services.SearchStockTickersResponse, Queries.SearchStockTickersResponse>();
            CreateMap<CreateStockTickerCommand, Domain.Entities.StockTicker>();
            CreateMap<UpdateStockTickerCommand, Domain.Entities.StockTicker>()
                .ForMember(d => d.Ticker, o => o.Ignore()); ;
        }
    }
}
