namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    public class GetCardsResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public List<CardObject> Cards { get; set; }
    }

    public enum CardStatus
    {
       Valid, ExpiresSoon, Expired
    }

    public class CardObject
    {
        public CardStatus CardStatus { get; set; }
        public string FullName { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string CVV { get; set; }
        public string Pin { get; set; }
    }
}
