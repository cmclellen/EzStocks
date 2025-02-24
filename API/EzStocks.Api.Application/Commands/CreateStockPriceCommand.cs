using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    public record CreateStockPriceItemCommand(Dtos.StockPriceItem StockItem) : IRequest;

    public class CreateStockPriceItemCommandHandler() : IRequestHandler<CreateStockPriceItemCommand>
    {
        public async Task Handle(CreateStockPriceItemCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            // TODO
            //var stockItem = mapper.Map<Dtos.StockItem, Domain.Entities.StockItem>(request.StockItem);
            //await stockRepository.CreateStockAsync(stockItem, cancellationToken);
            //await unitOfWork.Commit(cancellationToken);
        }
    }
}
