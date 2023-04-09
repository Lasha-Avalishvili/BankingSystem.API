using BankingSystem.DB.Entities;

namespace BankingSystem.Features.ATM.ChangePin
{
    public interface IChangePinService
    {
        public Task<ChangePinResponse> ChangePin(ChangePinRequest changeCardPINRequest);
    }
    public class ChangePinService : IChangePinService
    {
        private readonly IChangeCardPinRepository _changePinRepository;
        public ChangePinService(IChangeCardPinRepository changePinRepository)
        {
            _changePinRepository = changePinRepository;
        }
        public async Task<ChangePinResponse> ChangePin(ChangePinRequest request)
        {
            var response = new ChangePinResponse();
            try
            {
                var card = await _changePinRepository.AuthorizeCardAsync(request.CardNumber, request.PIN);
                if (card == null || request.NewPIN == card.PIN || request.NewPIN == null)
                {
                    throw new Exception("Incorrect credentials");
                }
                CheckCardExpiration(card);
                card.PIN = request.NewPIN;
                await _changePinRepository.SaveChangesAsync();
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = ex.Message;
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
