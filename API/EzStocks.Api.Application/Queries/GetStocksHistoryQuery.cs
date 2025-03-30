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
                stockPriceItem.Stocks[stock.Ticker] = stock.Close;
                return acc;
            });

            var stockTickerDtos = stockTickers.Select(_mapper.Map<Domain.Entities.StockTicker, Dtos.StockTicker>).ToList();

            return new GetStocksHistoryResponse(result, stockTickerDtos);
        }
    }
}
