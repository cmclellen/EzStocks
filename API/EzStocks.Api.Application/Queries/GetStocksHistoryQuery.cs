using Ardalis.Result;
using AutoMapper;
using EzStocks.Api.Application.Dtos;
using EzStocks.Api.Application.Security;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetStocksHistoryResponse(IList<StocksPriceItem> Prices, IList<Dtos.StockItem> Tickers);

    public record GetStocksHistoryQuery : IRequest<Result<GetStocksHistoryResponse>>;

    public class GetStocksHistoryQueryHandler(
        IMapper _mapper,
        IUserContext _userContext,
        IStockPriceItemRepository _stockPriceItemRepository,
        IStockItemRepository _stockItemRepository,
        IUserRepository _userRepository) : IRequestHandler<GetStocksHistoryQuery, Result<GetStocksHistoryResponse>>
    {
        public async Task<Result<GetStocksHistoryResponse>> Handle(GetStocksHistoryQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
            if(user is null)
            {
                return Result<GetStocksHistoryResponse>.NotFound();
            }

            var symbols = user.StockItems.Select(i=>i.Symbol).ToList();
            var stocks = await _stockPriceItemRepository.GetBySymbolsAsync(symbols, cancellationToken);

            var result = stocks.Aggregate(new List<StocksPriceItem>(), (acc, stock) =>
            {
                var stockPriceItem = acc.FirstOrDefault(i => i.CreatedDate == stock.AsAtDate);
                if(stockPriceItem is null)
                {
                    acc.Add(stockPriceItem = new StocksPriceItem
                    {
                        CreatedDate = stock.AsAtDate,
                        Stocks = new Dictionary<string, decimal>()
                    });
                }
                stockPriceItem.Stocks[stock.Symbol] = stock.Close;
                return acc;
            });

            var stockItems = await _stockItemRepository.GetBySymbolsAsync(symbols, cancellationToken);
            var stockItemsDtos = stockItems.Select(_mapper.Map<Domain.Entities.StockItem, Dtos.StockItem>).ToList();

            return new GetStocksHistoryResponse(result, stockItemsDtos);
        }
    }
}
