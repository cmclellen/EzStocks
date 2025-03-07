using Ardalis.Result;
using EzStocks.Api.Application.Dtos;
using EzStocks.Api.Application.Security;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetStocksHistoryResponse(IList<StocksPriceItem> Tickers, IList<string> Symbols);

    public record GetStocksHistoryQuery : IRequest<Result<GetStocksHistoryResponse>>;

    public class GetStocksHistoryQueryHandler(
        IUserContext _userContext,
        IStockPriceItemRepository _stockPriceItemRepository,
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

            return new GetStocksHistoryResponse(result, symbols);
        }
    }
}
