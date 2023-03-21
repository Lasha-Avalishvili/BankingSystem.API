using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.User.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Features.ATM.Withdraw
{
    public interface IWithdrawService
    {
        Task<WithdrawResponse> Withdraw(WithdrawRequest withdrawRequest);
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

        public async Task<WithdrawResponse> Withdraw(WithdrawRequest withdrawRequest)
        {
            var response = new WithdrawResponse();
            try
            {
                var senderAccount = await _withdrawRepository.GetSenderAccountAsync(withdrawRequest);
                var card = await _withdrawRepository.GetSenderAccountAsync(withdrawRequest);

                var transactions = await _withdrawRepository.GetCurrentDayTransactionsForUser(senderAccount.Id);
                var dailyLimitInGel = 10000;
                var convertedDailyLimit = _convertService.GetDailyLimitForEachCurrency(dailyLimitInGel, "GEL", senderAccount.Currency.ToString());
                decimal sumTransactions = 0;
                foreach (var a in transactions)
                {
                    sumTransactions += a.Amount;
                }


                if (convertedDailyLimit <= sumTransactions || withdrawRequest.Amount > convertedDailyLimit - sumTransactions)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "Something Wrong: your request out of daily limit";
                }
                else if(senderAccount == null || card == null)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "Incorrect ID or PIN";
                }
                else if (senderAccount.Balance <= 0 || withdrawRequest.Amount > senderAccount.Balance)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "Something wrong: Not Enught Money";
                }
                else
                {
                    var transaction = new TransactionEntity();
                    transaction.CreatedAt = DateTime.UtcNow;
                    transaction.SenderAccountId = senderAccount.Id;
                    transaction.RecipientAccountId = null;
                    transaction.Amount = withdrawRequest.Amount;
                    transaction.SenderAccount = senderAccount.IBAN;
                    transaction.RecipientAccount = null;
                    transaction.CurrencyFrom = senderAccount.Currency;
                    transaction.CurrencyTo = null;
                    transaction.ConvertRate = null;
                    var transactionFee = withdrawRequest.Amount * 2 / 100;
                    var convertedTransactionFeeInGel = await _convertService.ConvertCurrency(transactionFee, senderAccount.Currency.ToString(), "GEL");
                    var convertedTransactionFeeInUsd = await _convertService.ConvertCurrency(transactionFee, senderAccount.Currency.ToString(), "USD");
                    var convertedTransactionFeeInEur = await _convertService.ConvertCurrency(transactionFee, senderAccount.Currency.ToString(), "EUR");
                    transaction.FeeInGEL = convertedTransactionFeeInGel;
                    transaction.FeeInUSD = convertedTransactionFeeInUsd;
                    transaction.FeeInEUR = convertedTransactionFeeInEur;
                    senderAccount.Balance -= withdrawRequest.Amount + transactionFee;
                    transaction.TransactionType = TransactionType.ATM;

                    await _withdrawRepository.SaveChangesAsync(transaction);

                    response.IsSuccessful = true;
                    response.ErrorMessage = null;
                    response.Amount = transaction.Amount;
                    response.SenderAccount =  transaction.SenderAccount;
                    response.RecipientAccount = transaction.RecipientAccount;

                }
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
