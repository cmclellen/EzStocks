using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    //public record CreateStockCommand(Dtos.StockTicker StockItem) : IRequest;

    //public class CreateStockCommandHandler(
    //    IMapper _mapper,
    //    IStockTickerRepository _stockTickerRepository,
    //    IUnitOfWork _unitOfWork) : IRequestHandler<CreateStockCommand>
    //{
    //    public async Task Handle(CreateStockCommand request, CancellationToken cancellationToken)
    //    {
    //        var stockItem = _mapper.Map<Dtos.StockTicker, Domain.Entities.StockTicker>(request.StockItem);
    //        await _stockTickerRepository.CreateAsync(stockItem, cancellationToken);
    //        await _unitOfWork.CommitAsync(cancellationToken);
    //    }
    //}
}
