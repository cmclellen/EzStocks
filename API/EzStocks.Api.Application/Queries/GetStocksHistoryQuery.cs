using Ardalis.Result;
using EzStocks.Api.Application.Dtos;
using EzStocks.Api.Application.Security;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetStocksHistoryQuery : IRequest<Result<IList<StocksPriceItem>>>;

    public class GetStocksHistoryQueryHandler(
        IUserContext _userContext,
        IStockPriceItemRepository _stockPriceItemRepository,
        IUserRepository _userRepository,
        IStockHistoryItemRepository _stockHistoryRepository) : IRequestHandler<GetStocksHistoryQuery, Result<IList<StocksPriceItem>>>
    {
        public async Task<Result<IList<StocksPriceItem>>> Handle(GetStocksHistoryQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
            if(user is null)
            {
                return Result<IList<StocksPriceItem>>.NotFound();
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

                //{
                //["AAPL"] = 4000,
                //            ["GOOG"] = 2400,
                //        }

                return acc;
            });

            //var result = new List<StocksPriceItem>
            //{
            //    new StocksPriceItem
            //    {
            //        CreatedDate = new DateOnly(2025, 2, 1),
            //        Stocks = new Dictionary<string, decimal>
            //        {
            //            ["AAPL"] = 4000,
            //            ["GOOG"] = 2400,
            //        }
            //    },
            //    new StocksPriceItem
            //    {
            //        CreatedDate = new DateOnly(2025, 2, 2),
            //        Stocks = new Dictionary<string, decimal>
            //        {
            //            ["AAPL"] = 3000,
            //            ["GOOG"] = 1398,
            //        }
            //    },
            //    new StocksPriceItem
            //    {
            //        CreatedDate = new DateOnly(2025, 2, 3),
            //        Stocks = new Dictionary<string, decimal>{
            //            ["AAPL"] = 2000,
            //            ["GOOG"] = 9800,
            //        }
            //    },
            //    new StocksPriceItem                 {
            //        CreatedDate = new DateOnly(2025, 2, 4),
            //        Stocks = new Dictionary<string, decimal>{
            //            ["AAPL"] = 2780,
            //            ["GOOG"] = 3908,
            //        }
            //    },
            //    new StocksPriceItem                 {
            //        CreatedDate = new DateOnly(2025, 2, 5),
            //        Stocks = new Dictionary<string, decimal>{
            //            ["AAPL"] = 1890,
            //            ["GOOG"] = 4800,
            //        }
            //    },
            //    new StocksPriceItem                 {
            //        CreatedDate = new DateOnly(2025, 2, 6),
            //        Stocks = new Dictionary<string, decimal>{
            //            ["AAPL"] = 2390,
            //            ["GOOG"] = 3800,
            //        }
            //    },
            //    new StocksPriceItem                 {
            //        CreatedDate = new DateOnly(2025, 2, 7),
            //        Stocks = new Dictionary<string, decimal>{
            //            ["AAPL"] = 3490,
            //            ["GOOG"] = 4300,
            //        }
            //    },
            //};
            return result;
        }
    }
}
