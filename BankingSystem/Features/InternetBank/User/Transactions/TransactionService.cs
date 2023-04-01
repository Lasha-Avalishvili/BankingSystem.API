﻿using BankingSystem.DB;
using BankingSystem.DB.Entities;
using BankingSystem.Features.ATM.Withdraw;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace BankingSystem.Features.InternetBank.User.Transactions
{
    public interface ITransactionService
    {
        Task <TransactionResponse>TransferFunds (TransactionRequest transactionRequest, string authenticatedUseId);
    }
    public class TransactionService : ITransactionService
    {
        private readonly IConvertService _convertService;
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(IConvertService convertService, ITransactionRepository transactionRepository)
        {
            _convertService = convertService;
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionResponse> TransferFunds(TransactionRequest transactionRequest, string authenticatedUserId)
        {
            var response = new TransactionResponse();
            try
            {
               // await _convertService.UpdateRates();    //  atm-shi ar unda igive?
                var senderAccount = await _transactionRepository.GetAccountAsync(transactionRequest.SenderAccountIBAN); 
                var recipientAccount = await _transactionRepository.GetAccountAsync(transactionRequest.RecipientAccountIBAN); 

                var transaction = new TransactionEntity();  // jer ar momxdara transactioni
                transaction.CreatedAt = DateTime.UtcNow;
                transaction.SenderAccountId = senderAccount.Id;
                transaction.RecipientAccountId = recipientAccount.Id;
                transaction.Amount = transactionRequest.Amount;
                transaction.SenderAccount = transactionRequest.SenderAccountIBAN;
                transaction.RecipientAccount = transactionRequest.RecipientAccountIBAN;
                transaction.CurrencyFrom = senderAccount.Currency;
                transaction.CurrencyTo = recipientAccount.Currency;
                transaction.ConvertRate = await _convertService.GetRate(senderAccount.Currency.ToString(), recipientAccount.Currency.ToString());
                
                decimal transactionFee = 0;
                if (senderAccount.UserId != recipientAccount.UserId)
                {
                    transactionFee = transactionRequest.Amount * 1 / 100 + (Decimal)0.5;
                }

                if (senderAccount.UserId != int.Parse(authenticatedUserId))
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "You can operate only with your iban";
                    return response;
                }
                else if (senderAccount.Balance < transactionRequest.Amount + transactionFee) // added fee here 
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "Not Enough Money";
                    return response;
                }
                else if (senderAccount.UserId == recipientAccount.UserId)
                {
                    transaction.FeeInGEL = 0;
                    transaction.FeeInUSD = 0;
                    transaction.FeeInEUR = 0;
                    senderAccount.Balance -= transactionRequest.Amount;
                    var convertedAmount = await _convertService.ConvertCurrency(transactionRequest.Amount, senderAccount.Currency.ToString(), recipientAccount.Currency.ToString());
                    recipientAccount.Balance += convertedAmount;
                    transaction.TransactionType = TransactionType.Inner;
                }
                else if (senderAccount.UserId != recipientAccount.UserId)
                {
                    var convertedTransactionFeeInGel = await _convertService.ConvertCurrency(transactionFee, senderAccount.Currency.ToString(), "GEL");
                    var convertedTransactionFeeInUsd = await _convertService.ConvertCurrency(transactionFee, senderAccount.Currency.ToString(), "USD");
                    var convertedTransactionFeeInEur = await _convertService.ConvertCurrency(transactionFee, senderAccount.Currency.ToString(), "EUR");
                    transaction.FeeInGEL = convertedTransactionFeeInGel;
                    transaction.FeeInUSD = convertedTransactionFeeInUsd;
                    transaction.FeeInEUR = convertedTransactionFeeInEur;

                    senderAccount.Balance -= (transactionRequest.Amount + transactionFee);
                    var convertedAmount = await _convertService.ConvertCurrency(transactionRequest.Amount, senderAccount.Currency.ToString(), recipientAccount.Currency.ToString());
                    recipientAccount.Balance += convertedAmount;
                    transaction.TransactionType = TransactionType.Outer;
                }

                await _transactionRepository.SaveChangesAsync(transaction);

                response.IsSuccessful = true;
                response.ErrorMessage = null;
                response.Amount = transaction.Amount;
                response.SenderAccount = transaction.SenderAccount;
                response.RecipientAccount = transaction.RecipientAccount;
                response.CurrencyFrom = transaction.CurrencyFrom;
                response.CurrencyTo = transaction.CurrencyTo;
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
