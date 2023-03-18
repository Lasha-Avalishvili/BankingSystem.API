using BankingSystem.DB;
using BankingSystem.DB.Entities;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace BankingSystem.Features.InternetBank.User.Transactions
{
    public interface ITransactionService
    {
        Task <TransactionEntity>TransferFunds (TransactionRequest transactionRequest);
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

        public async Task<TransactionEntity> TransferFunds(TransactionRequest transactionRequest)
        {
            await _convertService.UpdateRates();
            var senderAccount =  await _transactionRepository.GetSenderAccountAsync(transactionRequest);
            var recipientAccount =  await _transactionRepository.GetRecipientAccountAsync(transactionRequest);

            var transaction = new TransactionEntity();
            transaction.CreatedAt = DateTime.UtcNow;
            transaction.SenderAccountId = senderAccount.Id;
            transaction.RecipientAccountId = recipientAccount.Id;   
            transaction.Amount = transactionRequest.Amount;
            transaction.SenderAccount = transactionRequest.SenderAccountIBAN;
            transaction.RecipientAccount = transactionRequest.RecipientAccountIBAN;
            transaction.CurrencyFrom = senderAccount.Currency;
            transaction.CurrencyTo = recipientAccount.Currency;
            transaction.ConvertRate = _convertService.GetRate(senderAccount.Currency.ToString(), recipientAccount.Currency.ToString());
            
            if (senderAccount.UserId == recipientAccount.UserId)
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
                var transactionFeeByPrecent = transactionRequest.Amount * 1 / 100;
                var transactionFee = transactionFeeByPrecent + (Decimal)0.5;
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
            return transaction;
        }
    }
}
