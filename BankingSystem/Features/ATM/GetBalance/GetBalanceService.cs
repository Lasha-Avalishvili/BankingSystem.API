using BankingSystem.DB.Entities;
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
            var response = new GetBalanceResponse();
            try
            {
                var card = await _getBalanceRepsoitory.GetCardAsync(request);
                if (card == null)
                {
                    throw new Exception("Invalid card number or PIN");
                }

                CheckCardExpiration(card);
                var account = card.Account;
                response.IsSuccessful = true;
                response.Balance=account.Balance;
                response.Currency=account.Currency;

            } 
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public void CheckCardExpiration(CardEntity card)
        {
            var isExpired = card.ExpirationDate < DateTime.UtcNow;
            if (isExpired)
            {
                throw new InvalidOperationException("Your card is Expired");
            }
        }

    }
}






