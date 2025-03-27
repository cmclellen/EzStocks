using Ardalis.GuardClauses;
using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EzStocks.Api.Application.Commands
{
    public record CreateStockTickerCommand : IRequest
    {
        public CreateStockTickerCommand(string ticker, string name, string color)
        {
            Ticker = Guard.Against.NullOrWhiteSpace(ticker, nameof(ticker));
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name)); ;
            Color = color;
        }

        public string Ticker { get; init; }
        public string Name { get; init; }
        public string Color { get; init; }
    }

    public class CreateStockTickersCommandHandler(
        ILogger<CreateStockTickersCommandHandler> _logger,
        IMapper mapper,
        IStockTickerRepository _stockTickerRepository,
        IUnitOfWork _unitOfWork) : IRequestHandler<CreateStockTickerCommand>
    {
        public async Task Handle(CreateStockTickerCommand request, CancellationToken cancellationToken)
        {
            using var scopeTicker = _logger.BeginScope(new Dictionary<string, object> { 
                ["Ticker"] = request.Ticker,
                ["TickerName"] = request.Name,
            });

            _logger.LogDebug("Creating stock ticker...");

            var existingStockTicker = await _stockTickerRepository.GetByTickersAsync([request.Ticker], cancellationToken);
            if(existingStockTicker.Any())
            {
                throw new Exception($"Stock ticker {request.Ticker} already exists");
            }

            var stockTickerToAdd = mapper.Map<CreateStockTickerCommand, Domain.Entities.StockTicker>(request);
            await _stockTickerRepository.AddAsync(stockTickerToAdd, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogDebug("Successfully Added stock ticker");
        }
    }
}
