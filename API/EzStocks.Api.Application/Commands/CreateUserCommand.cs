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
            var tickers = request.User.StockTickers.Select(item => item.Ticker).ToList();

            var stockTickers = await _stockTickerRepository.GetByTickersAsync(tickers, cancellationToken);
            
            var user = _mapper.Map<Dtos.User, Domain.Entities.User>(request.User);            
            user.StockTickers = stockTickers!.Select(item=>_mapper.Map<Domain.Entities.StockTicker, Domain.Entities.UserStockTicker>(item)).ToList();

            await _userRepository.CreateAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
