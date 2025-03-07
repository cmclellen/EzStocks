using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    public record CreateUserCommand(Application.Dtos.User User) : IRequest;

    public class CreateUserCommandHandler(
        IMapper _mapper,
        IUnitOfWork _unitOfWork,
        IUserRepository _userRepository,
        IStockItemRepository _stockItemRepository) : IRequestHandler<CreateUserCommand>
    {
        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var symbols = request.User.StockItems.Select(item => item.Symbol).ToList();
            var stockItems = await _stockItemRepository.GetBySymbolsAsync(symbols, cancellationToken);

            var user = _mapper.Map<Dtos.User, Domain.Entities.User>(request.User);            
            user.StockItems = stockItems!.Select(item=>_mapper.Map<Domain.Entities.StockItem, Domain.Entities.UserStockItem>(item)).ToList();

            await _userRepository.CreateAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
