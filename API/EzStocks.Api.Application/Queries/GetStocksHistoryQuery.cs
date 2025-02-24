using EzStocks.Api.Application.Dtos;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetStocksHistoryQuery : IRequest<IList<StocksPriceItem>>;

    public class GetStocksHistoryQueryHandler(IStockHistoryItemRepository stockHistoryRepository) : IRequestHandler<GetStocksHistoryQuery, IList<StocksPriceItem>>
    {
        public async Task<IList<StocksPriceItem>> Handle(GetStocksHistoryQuery request, CancellationToken cancellationToken)
        {
            var stockHistory = await stockHistoryRepository.GetStockHistoryAsync(cancellationToken);

            var result = new List<StocksPriceItem>
            {
                new StocksPriceItem
                {
                    CreatedDate = new DateOnly(2025, 2, 1),
                    Stocks = new Dictionary<string, decimal>
                    {
                        ["AAPL"] = 4000,
                        ["GOOG"] = 2400,
                    }
                },
                new StocksPriceItem
                {
                    CreatedDate = new DateOnly(2025, 2, 2),
                    Stocks = new Dictionary<string, decimal>
                    {
                        ["AAPL"] = 3000,
                        ["GOOG"] = 1398,
                    }
                },
                new StocksPriceItem
                {
                    CreatedDate = new DateOnly(2025, 2, 3),
                    Stocks = new Dictionary<string, decimal>{
                        ["AAPL"] = 2000,
                        ["GOOG"] = 9800,
                    }
                },
                new StocksPriceItem                 {
                    CreatedDate = new DateOnly(2025, 2, 4),
                    Stocks = new Dictionary<string, decimal>{
                        ["AAPL"] = 2780,
                        ["GOOG"] = 3908,
                    }
                },
                new StocksPriceItem                 {
                    CreatedDate = new DateOnly(2025, 2, 5),
                    Stocks = new Dictionary<string, decimal>{
                        ["AAPL"] = 1890,
                        ["GOOG"] = 4800,
                    }
                },
                new StocksPriceItem                 {
                    CreatedDate = new DateOnly(2025, 2, 6),
                    Stocks = new Dictionary<string, decimal>{
                        ["AAPL"] = 2390,
                        ["GOOG"] = 3800,
                    }
                },
                new StocksPriceItem                 {
                    CreatedDate = new DateOnly(2025, 2, 7),
                    Stocks = new Dictionary<string, decimal>{
                        ["AAPL"] = 3490,
                        ["GOOG"] = 4300,
                    }
                },
            };
            return result;
        }
    }
}
