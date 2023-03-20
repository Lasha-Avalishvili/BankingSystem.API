using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.ATM.ChangePin
{
    public interface IChangePinService
    {
        public Task<ChangePinResponse> ChangePin(ChangeCardPinRequest changeCardPINRequest, string authenticatedUserId);
    }
    public class ChangePinService : IChangePinService
    {
        private readonly IChangeCardPinRepository _changePinRepository;
        public ChangePinService(IChangeCardPinRepository changePinRepository)
        {
            _changePinRepository = changePinRepository;
        }
        public async Task<ChangePinResponse> ChangePin(ChangeCardPinRequest changeCardPinRequest, string authenticatedUserId)
        {
            var response = new ChangePinResponse();
            try
            {
                var card = await _changePinRepository.ChangePinAsync(changeCardPinRequest, authenticatedUserId);
                if (card != null && card.Account.UserId.ToString() == authenticatedUserId && changeCardPinRequest.NewPIN != card.PIN && changeCardPinRequest.NewPIN != null)
                {
                    card.PIN = changeCardPinRequest.NewPIN;


                    response.IsSuccessful = true;
                    response.Error = null;
                    await _changePinRepository.SaveChangesAsync();
                }
            }catch(Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = ex.Message;
            }
            return response;
         
        }
    }
}
