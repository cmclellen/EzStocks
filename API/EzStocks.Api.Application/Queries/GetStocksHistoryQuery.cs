﻿using Ardalis.Result;
using AutoMapper;
using EzStocks.Api.Application.Dtos;
using EzStocks.Api.Application.Security;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetStocksHistoryResponse(IList<StocksPriceItem> Prices, IList<Dtos.StockTicker> Tickers);

    public record GetStocksHistoryQuery : IRequest<Result<GetStocksHistoryResponse>>;

    public class GetStocksHistoryQueryHandler(
        IMapper _mapper,
        IUserContext _userContext,
        IStockPriceItemRepository _stockPriceItemRepository,
        IStockTickerRepository _stockTickerRepository,
        IUserRepository _userRepository) : IRequestHandler<GetStocksHistoryQuery, Result<GetStocksHistoryResponse>>
    {
        public async Task<Result<GetStocksHistoryResponse>> Handle(GetStocksHistoryQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
            if(user is null)
            {
                return Result<GetStocksHistoryResponse>.NotFound();
            }

            var tickers = user.StockTickers.Select(i=>i.Ticker).ToList();
            var stocks = await _stockPriceItemRepository.GetByTickersAsync(tickers, cancellationToken);

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

            // TODO: Get user stock tickers
            var userStockTickers = new List<Dtos.StockTicker>();
            //var stockItems = await _stockTickerRepository.GetBySymbolsAsync(symbols, cancellationToken);
            //var stockItemsDtos = stockItems.Select(_mapper.Map<Domain.Entities.StockTicker, Dtos.StockTicker>).ToList();

            return new GetStocksHistoryResponse(result, userStockTickers);
        }
    }
}
