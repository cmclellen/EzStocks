using Ardalis.Result;
using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    public record UpdateStockTickerCommand(string Ticker, string Name, string Color) : IRequest<Result>;

    public class UpdateStockTickerCommandHandler : IRequestHandler<UpdateStockTickerCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IStockTickerRepository _stockTickerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStockTickerCommandHandler(
            IMapper mapper,
            IStockTickerRepository stockTickerRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _stockTickerRepository = stockTickerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateStockTickerCommand request, CancellationToken cancellationToken)
        {
            var stockTickers = await _stockTickerRepository.GetByTickersAsync([request.Ticker], cancellationToken);
            if (stockTickers.Count != 1) return Result.NotFound();

            var stockTicker = _mapper.Map(request, stockTickers[0]);
            await _stockTickerRepository.UpdateAsync(stockTicker, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}
