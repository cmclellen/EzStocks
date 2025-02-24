using AutoMapper;
using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetStocksQuery : IRequest<IList<Dtos.StockItem>>;

    public class GetStocksQueryHandler(
        IMapper mapper,
        IStockItemRepository stockRepository) : IRequestHandler<GetStocksQuery, IList<Dtos.StockItem>>
    {
        public async Task<IList<Dtos.StockItem>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            var stockEntities = await stockRepository.GetStocksAsync(cancellationToken);

            IList<Dtos.StockItem> stockItems = stockEntities.Select(entity => mapper.Map<StockItem, Dtos.StockItem>(entity)).ToList();

            return stockItems;
        }
    }
}
