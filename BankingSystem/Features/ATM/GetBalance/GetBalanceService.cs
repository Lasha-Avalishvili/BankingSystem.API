using BankingSystem.Features.ATM.AccountBlance;

namespace BankingSystem.Features.ATM.GetBalance
{
    public interface IGetBalanceService
    {
        public Task<GetBalanceResponse> GetBalance(GetBalanceRequest request);
    }

    public class GetBalanceService : IGetBalanceService
    {
        private readonly IGetBalanceRepository _getBalanceRepsoitory;
        public GetBalanceService(IGetBalanceRepository getBalanceRepository)
        {
            _getBalanceRepsoitory = getBalanceRepository;
        }

        public async Task<GetBalanceResponse> GetBalance (GetBalanceRequest request)
        {
            var card = await _getBalanceRepsoitory.GetBalanceAsync(request);

            if (card == null)
            {
                return new GetBalanceResponse
                {
                    IsSuccessful = false,
                    ErrorMessage = "Invalid card number or PIN"
                };
            }

            var account = card.Account;

            return new GetBalanceResponse
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Balance = account.Balance,
                Currency = account.Currency
            };
        }
    }
}






