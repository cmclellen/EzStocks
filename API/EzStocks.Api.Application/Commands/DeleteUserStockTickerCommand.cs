using Ardalis.Result;
using EzStocks.Api.Application.Security;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    public record DeleteUserStockTickerCommand(string Ticker) : IRequest<Result>;

    public class DeleteUserStockTickerCommandHandler : IRequestHandler<DeleteUserStockTickerCommand, Result>
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public DeleteUserStockTickerCommandHandler(
            IUserContext userContext,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            _userContext = userContext;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(DeleteUserStockTickerCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
            if(user is null)
            {
                return Result.NotFound("User not found");
            }
            var userStockTicker = user.StockTickers.FirstOrDefault(st => st.Ticker == request.Ticker);
            if(userStockTicker is null)
            {
                return Result.NotFound("User stock ticker not found");
            }
            user.StockTickers.Remove(userStockTicker);
            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success();
        }
    }
}
