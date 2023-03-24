using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.User.Transactions;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.ATM.Withdraw
{
    public interface IWithdrawService
    {
        public Task<WithdrawResponse> WithdawFromAtm(WithdrawFromAtmRequest request);
    }

    public class WithdrawService : IWithdrawService
    {
        private readonly IConvertService _convertService;
        private readonly IWithdrawRepository _withdrawRepository;
        public WithdrawService(IConvertService convertService, IWithdrawRepository withdrawRepository)
        {
            _convertService = convertService;
            _withdrawRepository = withdrawRepository;
        }

        public async Task<WithdrawResponse> WithdawFromAtm(WithdrawFromAtmRequest request)
        {
            var response = new WithdrawResponse();
            try
            {
                var card = await _withdrawRepository.AuthorizeCardAsync(request.CardNumber, request.PIN);
                if (card == null)
                {
                    throw new Exception("Incorrect Card Credentials");
                }
                var account = await _withdrawRepository.FindAccountAsync(card.AccountId);
                var accountBalance = account.Balance;

                decimal requestedAmountInCardCurrency = await _convertService.ConvertCurrency(request.Amount, request.Currency.ToString(), account.Currency.ToString());

                var transactionFee = requestedAmountInCardCurrency / 50;
                var requestedAmountAndFee = requestedAmountInCardCurrency + transactionFee;

                if (requestedAmountAndFee > accountBalance)
                {
                    throw new Exception("Not enought money");
                }

                var requestedAmountInGel = await _convertService.ConvertCurrency(request.Amount, request.Currency.ToString(), "GEL");
                var AtmTransactionsAmountInGel = await _withdrawRepository.GetUserAtmTransactions(account.UserId, Currency.GEL);
                var AtmTransactionsAmountInUSD = await _withdrawRepository.GetUserAtmTransactions(account.UserId, Currency.USD);
                var AtmTrasactionInUSDConverted = await _convertService.ConvertCurrency(AtmTransactionsAmountInUSD, "USD", "GEL");
                var AtmTransactionsAmountInEUR = await _withdrawRepository.GetUserAtmTransactions(account.UserId, Currency.EUR);
                var AtmTransactionInEURConverted = await _convertService.ConvertCurrency(AtmTransactionsAmountInUSD, "EUR", "GEL");

                var allAtmTransactionsInGel = AtmTransactionsAmountInGel + AtmTrasactionInUSDConverted + AtmTransactionInEURConverted;
                var dailyLimitInGel = 10000;
                if(dailyLimitInGel< allAtmTransactionsInGel + requestedAmountInGel)
                {
                    throw new Exception("You are above your daily limit");
                }

                account.Balance -= requestedAmountAndFee;

                var transaction = new TransactionEntity();
                transaction.Amount = request.Amount;
                transaction.CurrencyFrom = account.Currency;
                transaction.CurrencyTo = request.Currency;
                transaction.CreatedAt = DateTime.UtcNow;
                transaction.SenderAccount = account.IBAN;
                transaction.FeeInGEL = await _convertService.ConvertCurrency(transactionFee, account.Currency.ToString(), "GEL");
                transaction.FeeInUSD = await _convertService.ConvertCurrency(transactionFee, account.Currency.ToString(), "USD");
                transaction.FeeInEUR = await _convertService.ConvertCurrency(transactionFee, account.Currency.ToString(), "EUR");
                transaction.TransactionType = TransactionType.ATM;
                transaction.SenderAccountId = account.Id; 
                transaction.ConvertRate = _convertService.GetRate(account.Currency.ToString(), request.Currency.ToString());

                await _withdrawRepository.SaveChangesAsync(transaction); 

                response.IsSuccessful = true;
                response.ErrorMessage = null;
                response.Amount = transaction.Amount;
                response.AccountIBAN = transaction.SenderAccount;
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
