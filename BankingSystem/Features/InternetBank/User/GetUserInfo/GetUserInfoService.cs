using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.Operator.AuthUser;
using BankingSystem.Features.InternetBank.Operator.RegisterUser;
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

        public async Task<List<GetAccountsResponse>> GetAccountsAsync(string authenticatedUserId)
        {
            var response = new List<GetAccountsResponse>();

            var account = await _repository.GetUserAccountsAsync(authenticatedUserId);
            bool authorized = account.Any(a => a.UserId.ToString() == authenticatedUserId);
            try
            {
                if (authorized)
                {
                    var accounts = await _repository.GetUserAccountsAsync(authenticatedUserId);
                    response = accounts.Select(a => new GetAccountsResponse
                    {
                        IsSuccessful = true,
                        Error = null,
                        AccountId = a.Id,
                        IBAN = a.IBAN,
                        Balance = a.Balance,
                        Currency = a.Currency
                    }).ToList();
                }
                else
                {
                    response.Add(new GetAccountsResponse
                    {
                        IsSuccessful = false,
                        Error = "Unauthorized access"
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        public async Task<List<GetCardsResponse>> GetCardsAsync(string authenticatedUserId, int accountId)
        {
            var response = new List<GetCardsResponse>();
            try
            {
                var accounts = await _repository.GetUserAccountsAsync(authenticatedUserId);
                foreach (var account in accounts)
                {
                    if (account.UserId == int.Parse(authenticatedUserId))
                    {
                        var cards = await _repository.GetUserCardsAsync(authenticatedUserId, accountId);

                        response = cards.Select(a => new GetCardsResponse
                        {
                            FullName = a.FullName,
                            CardNumber = a.CardNumber,
                            ExpirationDate = a.ExpirationDate,
                            CVV = a.CVV,
                            Pin = a.PIN
                        }
                        ).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                new Exception(ex.Message); // ??
            }
            return response;

        }

        public async Task<List<GetTransactionsResponse>> GetTransactionsAsync( string IBAN, string authenticatedUserId)
        {
            var response = new List<GetTransactionsResponse>();

            try
            {
                // we need validation so user cant check other people's transactions
                var transactions = await _repository.GetUserAccountTransactionsAsync(IBAN);
                response = transactions.Select(a => new GetTransactionsResponse
                {
                   TransactionDate = a.CreatedAt,
                   TransactionType= a.TransactionType,
                   Amount = a.Amount,
                   SenderAccount = a.SenderAccount,
                   RecipientAccount =a.RecipientAccount
                }
                ).ToList();

            }
            catch (Exception ex)
            {
                new Exception(ex.Message); // ??
            }
            return response;

        }
    }
}
