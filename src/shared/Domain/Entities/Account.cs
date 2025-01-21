namespace Domain.Entities
{
    public class Account : User
    {
        public double AccId { get; set; }
        public string? AccStatus { get; set; }
        public long? Balance { get; set; }


        public Account()
        {
            AccId = 12345;
            AccStatus = "Active";
            Balance = 10000;
        }



        // for testing purposes
        public Account(double accId, string accStatus, long balance)
        {
            AccId = accId;
            AccStatus = accStatus;
            Balance = balance;
        }
    }
}