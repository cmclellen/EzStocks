using Ardalis.Result;
using AutoMapper;
using EzStocks.Api.Application.Security;
using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    public record AddUserStockTickerCommand(string Ticker) : IRequest<Result>;

    public class AddUserStockTickerCommandHandler: IRequestHandler<AddUserStockTickerCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IStockTickerRepository _stockTickerRepository;

        public AddUserStockTickerCommandHandler(
            IMapper mapper,
            IUserContext userContext,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IStockTickerRepository stockTickerRepository)
        {
            _mapper = mapper;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _stockTickerRepository = stockTickerRepository;
        }

        public async Task<Result> Handle(AddUserStockTickerCommand request, CancellationToken cancellationToken)
        {
            var stockTicker = await _stockTickerRepository.GetByTickersAsync([request.Ticker], cancellationToken);
            if(stockTicker is null)
            {
                return Result.NotFound("Stock ticker not found");
            }
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
            if (user is null)
            {
                return Result.NotFound("User not found");
            }
            var userStockTicker = _mapper.Map<Domain.Entities.StockTicker, UserStockTicker>(stockTicker.First());
            user.StockTickers.Add(userStockTicker);

            await _userRepository.UpdateAsync(user, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}
