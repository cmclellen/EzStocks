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
        IStockTickerRepository _stockTickerRepository) : IRequestHandler<CreateUserCommand>
    {
        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var symbols = request.User.StockItems.Select(item => item.Symbol).ToList();

            var stockTickers = await _stockTickerRepository.GetByTickersAsync(symbols, cancellationToken);
            
            var user = _mapper.Map<Dtos.User, Domain.Entities.User>(request.User);            
            user.StockTickers = stockTickers!.Select(item=>_mapper.Map<Domain.Entities.StockTicker, Domain.Entities.UserStockTicker>(item)).ToList();

            await _userRepository.CreateAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
