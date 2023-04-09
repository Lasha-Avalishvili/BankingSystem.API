using BankingSystem.DB.Entities;
using BankingSystem.Features.InternetBank.User.Transactions;

namespace BankingSystem.Features.ATM.Withdraw
{
    public interface IWithdrawService
    {
        public Task<WithdrawResponse> WithdawFromAtm(WithdrawRequest request);
    }

    public class WithdrawService : IWithdrawService
    {
        private readonly IConvertService _convertService;
        private readonly IWithdrawRepository _withdrawRepository;
        private readonly decimal WITHDRAWALLIMITINGEL = 10000;
        private readonly decimal WITHDRAWALFEE = 0.02m;
        public WithdrawService(IConvertService convertService, IWithdrawRepository withdrawRepository)
        {
            _convertService = convertService;
            _withdrawRepository = withdrawRepository;
        }

        public async Task<WithdrawResponse> WithdawFromAtm(WithdrawRequest request)
        {
            var response = new WithdrawResponse();
            try
            {
                var card = await _withdrawRepository.AuthorizeCardAsync(request.CardNumber, request.PIN);
                if (card == null)
                {
                    throw new Exception("Incorrect card credentials");
                }
                CheckCardExpiration(card);

                var account = await _withdrawRepository.FindAccountAsync(card.AccountId);
                var accountBalance = account.Balance;

                decimal requestedAmountInCardCurrency = await _convertService.ConvertCurrency(request.Amount, request.Currency.ToString(), account.Currency.ToString());

                var transactionFee = requestedAmountInCardCurrency * WITHDRAWALFEE;
                var requestedAmountAndFee = requestedAmountInCardCurrency + transactionFee;

                if (requestedAmountAndFee > accountBalance)
                {
                    throw new Exception("Not enought money");
                }

                await CheckWithdrawalLimit(request, account, WITHDRAWALLIMITINGEL);

                account.Balance -= requestedAmountAndFee;

                var transaction = await CreateTransactionEntity(request, account, transactionFee);
                await _withdrawRepository.AddChangesAsync(transaction); 
                await _withdrawRepository.SaveChangesAsync();

                response.IsSuccessful = true;
                response.Amount = transaction.Amount;
                response.AccountIBAN = transaction.SenderAccount;
                response.CurrencyTo = transaction.CurrencyTo;
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

        public async Task CheckWithdrawalLimit(WithdrawRequest request, AccountEntity account, decimal dailyLimit)
        {
            var requestedAmountInGel = await _convertService.ConvertCurrency(request.Amount, request.Currency.ToString(), "GEL");
            var AtmTransactionsAmountInGel = await _withdrawRepository.GetUserAtmTransactions(account.UserId, Currency.GEL);
            var AtmTransactionsAmountInUSD = await _withdrawRepository.GetUserAtmTransactions(account.UserId, Currency.USD);
            var AtmTrasactionInUSDConverted = await _convertService.ConvertCurrency(AtmTransactionsAmountInUSD, "USD", "GEL");
            var AtmTransactionsAmountInEUR = await _withdrawRepository.GetUserAtmTransactions(account.UserId, Currency.EUR);
            var AtmTransactionInEURConverted = await _convertService.ConvertCurrency(AtmTransactionsAmountInEUR, "EUR", "GEL");

            var allAtmTransactionsInGel = AtmTransactionsAmountInGel + AtmTrasactionInUSDConverted + AtmTransactionInEURConverted;
            var dailyLimitInGel = dailyLimit;
            if (dailyLimitInGel < allAtmTransactionsInGel + requestedAmountInGel)
            {
                throw new Exception("Daily withdrawal limit is not enough");
            }
        }

        private async Task<TransactionEntity> CreateTransactionEntity(WithdrawRequest request, AccountEntity account, decimal transactionFee)
        {
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
            transaction.ConvertRate = await _convertService.GetRate(account.Currency.ToString(), request.Currency.ToString());
            transaction.AmountInGEL = await _convertService.ConvertCurrency(request.Amount, request.Currency.ToString(), "GEL");
            return transaction;
        }

    }
}
