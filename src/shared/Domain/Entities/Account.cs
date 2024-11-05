namespace Domain.Entities
{
    public class Account : User
    {
        public double AccId { get; set; }
        public string? AccStatus { get; set; }
        public string? Security { get; set; }


        public Account()
        {
            AccId = 12345;
            AccStatus = "Active";
            Security = "NotCompromised";
        }

        public Account(double accId, string accStatus, string security)
        {
            AccId = accId;
            AccStatus = accStatus;
            Security = security;
        }

    }


}