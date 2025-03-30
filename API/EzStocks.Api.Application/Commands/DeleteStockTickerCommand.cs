using Ardalis.Result;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    public record DeleteStockTickerCommand(string Ticker) : IRequest<Result>;

    public class DeleteStockTickerCommandHandler : IRequestHandler<DeleteStockTickerCommand, Result>
    {
        private readonly IStockTickerRepository _stockTickerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStockTickerCommandHandler(
            IStockTickerRepository stockTickerRepository,
            IUnitOfWork unitOfWork)
        {
            _stockTickerRepository = stockTickerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteStockTickerCommand request, CancellationToken cancellationToken)
        {
            var stockTickers = await _stockTickerRepository.GetByTickersAsync([request.Ticker], cancellationToken);
            if (stockTickers.Count != 1) return Result.NotFound();

            await _stockTickerRepository.DeleteAsync(stockTickers[0], cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}
