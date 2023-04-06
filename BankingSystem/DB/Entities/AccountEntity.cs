namespace BankingSystem.DB.Entities
{
    public enum Currency
    {
        GEL,
        USD,
        EUR
   
    }
    public class AccountEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }  
        public UserEntity User { get; set; }    
        public string IBAN { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
        public List<CardEntity> Cards { get; set; }
    }
}
