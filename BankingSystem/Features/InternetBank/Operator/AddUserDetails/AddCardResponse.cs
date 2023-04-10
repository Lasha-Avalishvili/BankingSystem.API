namespace BankingSystem.Features.InternetBank.Operator.AddUserDetails
{
    public class AddCardResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public int CardId { get; set; }
    }
}
