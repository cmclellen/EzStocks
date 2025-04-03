using Ardalis.Result;
using AutoMapper;
using EzStocks.Api.Application.Dtos;
using EzStocks.Api.Application.Security;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetStocksHistoryResponse(IList<StocksPriceItem> Prices, IList<Dtos.StockTicker> StockTickers);

    public record GetStocksHistoryQuery : IRequest<Result<GetStocksHistoryResponse>>;

    public class GetStocksHistoryQueryHandler : IRequestHandler<GetStocksHistoryQuery, Result<GetStocksHistoryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IStockPriceItemRepository _stockPriceItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStockTickerRepository _stockTickerRepository;

        public GetStocksHistoryQueryHandler(
            IMapper mapper,
            IUserContext userContext,
            IStockPriceItemRepository stockPriceItemRepository,
            IUserRepository userRepository,
            IStockTickerRepository stockTickerRepository)
        {
            _mapper = mapper;
            _userContext = userContext;
            _stockPriceItemRepository = stockPriceItemRepository;
            _userRepository = userRepository;
            _stockTickerRepository = stockTickerRepository;
        }

        public async Task<Result<GetStocksHistoryResponse>> Handle(GetStocksHistoryQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
            if(user is null)
            {
                return Result<GetStocksHistoryResponse>.NotFound();
            }

            var tickers = user.StockTickers.Select(i=>i.Ticker).ToList();
            var stocks = await _stockPriceItemRepository.GetByTickersAsync(tickers, cancellationToken);
            var stockTickers = await _stockTickerRepository.GetByTickersAsync(tickers, cancellationToken);

            Dictionary<string, StockMinMax> stockMinMaxList = new Dictionary<string, StockMinMax>();
            var stocksPriceItems = stocks.Aggregate(new List<StocksPriceItem>(), (acc, stock) =>
            {
                var stockPriceItem = acc.FirstOrDefault(i => i.CreatedDate == stock.AsAtDate);
                if(stockPriceItem is null)
                {
                    acc.Add(stockPriceItem = new StocksPriceItem
                    {
                        CreatedDate = stock.AsAtDate,
                        Stocks = new Dictionary<string, decimal>(),
                        PricePercentages = new Dictionary<string, double>(),
                    });
                }
                var price = stock.Close;
                stockPriceItem.Stocks[stock.Ticker] = price;

                AddStockMinMax(stockMinMaxList, stock.Ticker, price);

                return acc;
            });

            foreach(var stocksPriceItem in stocksPriceItems)
            {
                foreach(var ticker in stocksPriceItem.Stocks.Keys)
                {
                    var price = stocksPriceItem.Stocks[ticker];
                    var minMax = stockMinMaxList[ticker];
                    var max = minMax.Max - minMax.Min;
                    stocksPriceItem.PricePercentages[ticker] = max == 0 ? 1.0 : Convert.ToDouble(Math.Round((price - minMax.Min) / max, 2));
                }
            }

            var stockTickerDtos = stockTickers.Select(_mapper.Map<Domain.Entities.StockTicker, Dtos.StockTicker>).ToList();

            return new GetStocksHistoryResponse(stocksPriceItems, stockTickerDtos);
        }

        private void AddStockMinMax(Dictionary<string, StockMinMax> stockMinMaxList, string ticker, decimal price)
        {
            if(stockMinMaxList.TryGetValue(ticker, out var stockMinMax))
            {
                var max = Math.Max(stockMinMax.Max, price);
                var min = Math.Min(stockMinMax.Min, price);
                stockMinMaxList[ticker] = new StockMinMax(min, max);
            } 
            else
            {
                stockMinMaxList[ticker] = new StockMinMax(price, price);
            }
        }

        record struct StockMinMax(decimal Min, decimal Max);
    }
}
