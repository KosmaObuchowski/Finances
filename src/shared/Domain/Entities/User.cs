using System.Net.NetworkInformation;
using System.Reflection;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public double UserGovtId { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }

        public User()
        {
            UserId = 1;
            UserGovtId = 12345678900;
            Login = "JanKow1234";
            Name = "Jan";
            LastName = "Kowalski";
            DOB = "01.02.2000";
            Status = "Active";
            Password = "1234";
        }

        public User(int userId, double userGovtId, string login, string name, string lastName, string dob, string status, string password)
        {
            UserId = userId;
            UserGovtId = userGovtId;
            Login = login;
            Name = name;
            LastName = lastName;
            DOB = dob;
            Status = status;
            Password = password;
        }
    }
}