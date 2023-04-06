using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Operator.AuthUser;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;
using BankingSystem.Features.InternetBank.User.Transactions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    public class GetUserInfoService
    {
        private readonly GetUserInfoRepository _repository;
        public GetUserInfoService(GetUserInfoRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetAccountsResponse> GetAccountsAsync(string authenticatedUserId)
        {
            var response = new GetAccountsResponse();

            var account = await _repository.GetUserAccountsAsync(authenticatedUserId);
            bool authorized = account.Any(a => a.UserId.ToString() == authenticatedUserId);
            try
            {
                if (authorized == false)
                {
                    throw new Exception("Denied access to this account");
                }
                var accounts = await _repository.GetUserAccountsAsync(authenticatedUserId);
              
                if (accounts.Count == 0)
                {
                    throw new Exception("No accounts");
                }
                var accountObjects = accounts.Select(a => new AccountObject
                {
                    AccountId = a.Id,
                    IBAN = a.IBAN,
                    Balance = a.Balance,
                    Currency = a.Currency
                }).ToList();

                response.Accounts = accountObjects;
                response.IsSuccessful= true;
               
            }
            catch (Exception ex)
            {
                response.ErrorMessage= ex.Message;
                response.IsSuccessful= false;
            }
            return response;
        }

        public async Task<GetCardsResponse> GetCardsAsync(string authenticatedUserId, string Iban)
        {
            var response = new GetCardsResponse();
            try
            {
                var cards = await _repository.GetUserCardsAsync(authenticatedUserId, Iban);

                var cardObjects = cards.Select(a => new CardObject
                {
                    CardStatus = GetCardStatus(a.ExpirationDate),
                    FullName = a.FullName,
                    CardNumber = a.CardNumber,
                    ExpirationDate = a.ExpirationDate,
                    CVV = a.CVV,
                    Pin = a.PIN
                }
                ).ToList();
                if (cardObjects.Count == 0)
                {
                    throw new Exception("No cards with this IBAN");
                }

                response.IsSuccessful = true;
                response.Cards = cardObjects;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = ex.Message;

            }
            return response;

        }

        public async Task<GetTransactionsResponse> GetTransactionsAsync(string IBAN, string authenticatedUserId)
        {
            var response = new GetTransactionsResponse();

            try
            {
                var account = await _repository.GetAccountWithIban(IBAN);
                if (account == null)
                {
                    throw new Exception("Incorrect IBAN");
                }

                if (authenticatedUserId != account.UserId.ToString())
                {
                    throw new Exception("No access to this IBAN");
                }

                var transactions = await _repository.GetTransactionsWithIban(IBAN);

                if (transactions == null)
                {
                    throw new Exception("No Transactions with this IBAN");
                }

                response.TransactionReponse = transactions.Select(a => new TransactionObject
                {
                    TransactionDate = a.CreatedAt,
                    TransactionType = a.TransactionType,
                    Amount = a.Amount,
                    SenderAccount = a.SenderAccount,
                    RecipientAccount = a.RecipientAccount
                }
                ).ToList();

                response.IsSuccessful = true;

            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
        private CardStatus GetCardStatus(DateTime expirationDate)
        {
            var daysUntilExpiration = (expirationDate - DateTime.UtcNow).TotalDays;
            if (daysUntilExpiration <= 0)
            {
                return CardStatus.Expired;
            }
            else if (daysUntilExpiration <= 90)
            {
                return CardStatus.ExpiresSoon;
            }
            else
            {
                return CardStatus.Valid;
            }
        }
    }
}
