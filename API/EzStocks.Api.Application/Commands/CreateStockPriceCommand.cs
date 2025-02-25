using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    public record CreateStockPriceItemCommand(Dtos.StockPriceItem StockItem) : IRequest;

    public class CreateStockPriceItemCommandHandler(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IStockPriceItemRepository stockPriceItemRepository) : IRequestHandler<CreateStockPriceItemCommand>
    {
        public async Task Handle(CreateStockPriceItemCommand request, CancellationToken cancellationToken)
        {
            var stockPriceItem = mapper.Map<Dtos.StockPriceItem , Domain.Entities.StockPriceItem>(request.StockItem);
            await stockPriceItemRepository.CreateAsync(stockPriceItem, cancellationToken);            
            await unitOfWork.Commit(cancellationToken);
        }
    }
}
