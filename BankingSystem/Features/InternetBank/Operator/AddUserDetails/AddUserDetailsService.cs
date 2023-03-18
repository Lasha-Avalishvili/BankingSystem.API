using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Operator.AddAccountForUser;
using BankingSystem.Features.InternetBank.Operator.AddUser;
using BankingSystem.Features.InternetBank.Operator.AuthOperator;

namespace BankingSystem.Features.InternetBank.Operator.AddUserDetails
{
    public class AddUserDetailsService
    {
        private readonly IAddUserDetailsRepository _repository;
        public AddUserDetailsService(IAddUserDetailsRepository repository)
        {
            _repository = repository;
        }

        public async Task<AddAccountResponse> AddAccountAsync(AddAccountRequest request)
        {
            var response = new AddAccountResponse();

            try
            {
                var newAccount = new AccountEntity();
                newAccount.UserId = request.UserId;
                newAccount.IBAN = request.IBAN;
                newAccount.Currency = request.Currency;
                newAccount.Balance = request.Amount;

                // Additional business logic related to registration can go here
                // For example, validating the request data, sending a confirmation email to the user, etc.

                await _repository.AddAccountAsync(newAccount);

                response.IsSuccessful = true;
                //response.FirstName = newAccount.User.FirstName;
                //response.LastName = newAccount.User.LastName;
                //response.UserId = newAccount.User.Id;
                response.AccountId = newAccount.Id;

            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async Task<AddCardResponse> AddCardAsync(AddCardRequest request)
        {
            var response = new AddCardResponse();

            try
            {
                var newCard = new CardEntity();
                newCard.AccountId = request.AccountId;
                newCard.FullName = request.FullName;
                newCard.CardNumber = request.CardNumber;
                newCard.ExpirationDate = request.ExpirationDate;
                newCard.CVV = request.CVV;
                newCard.PIN = request.PIN;

                // Additional business logic related to registration can go here
                // For example, validating the request data, sending a confirmation email to the user, etc.

                await _repository.AddCardAsync(newCard);

                response.IsSuccessful = true;
                //response.FirstName = newCard.Account.User.FirstName;  // is this okay? :) do we need? 
                //response.LastName = newCard.Account.User.LastName;
                //response.UserId = newCard.Account.User.Id;
                //response.AccountId = newCard.Account.Id;
                response.CardId = newCard.Id;

            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }


    }
}
