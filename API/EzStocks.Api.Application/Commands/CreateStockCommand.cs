using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    public record CreateStockCommand(Dtos.StockItem StockItem) : IRequest;

    public class CreateStockCommandHandler(
        IMapper mapper,
        IStockItemRepository stockRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateStockCommand>
    {
        public async Task Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            var stockItem = mapper.Map<Dtos.StockItem, Domain.Entities.StockItem>(request.StockItem);
            await stockRepository.CreateStockAsync(stockItem, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
