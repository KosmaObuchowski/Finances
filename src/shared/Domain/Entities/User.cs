using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public long UserGovtId { get; set; }
        public string UserLogin { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }

        public User() { }

        // simulated login system
        public bool VerifyLogin(string login, string password)
        {
            return this.UserLogin == login && this.Password == password;
        }

        public (User, Account) Login()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string userJson = File.ReadAllText(@"../../../../files/user.json");
            List<User> user = JsonSerializer.Deserialize<List<User>>(userJson, options);

            if (user == null)
            {
                Console.WriteLine("User does not exist. Would you like to create an account?");
                CreateAccount();
            }

            Console.WriteLine("Login: ");
            string login = Console.ReadLine();
            Console.WriteLine("Password: ");
            string password = Console.ReadLine();

            User currentUser = user.Find(user => user.UserLogin == login);

            if (currentUser != null && currentUser.VerifyLogin(login, password))
            {
                Console.WriteLine("Welcome " + currentUser.Name + " " + currentUser.LastName);
                System.Threading.Thread.Sleep(1000);

                string accountJson = File.ReadAllText(@"../../../../files/account.json");
                List<Account> account = JsonSerializer.Deserialize<List<Account>>(accountJson, options);
                Account userAccount = account.Find(account => account.UserId == currentUser.UserId);

                if (userAccount != null)
                {
                    Console.Clear();
                    Console.WriteLine("...");
                    System.Threading.Thread.Sleep(1000);
                    return (currentUser, userAccount);
                }
                else
                {
                    Console.WriteLine("No account found");
                    System.Threading.Thread.Sleep(1000);
                    return Login();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Provide a correct password, silly!");
                return Login();
            }
        }

        // money withdrawal
        public Account Withdraw(Account userAccount)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Withdrawal in process");

            if (userAccount == null)
            {
                Console.WriteLine("Account not found");
                return null;
            }

            Console.WriteLine($"Current balance: {userAccount.Balance}");
            Console.WriteLine("Withdrawal amount: ");
            int amount;
            try
            {
                amount = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Enter a valid number, and not a letter.");
                System.Threading.Thread.Sleep(1000);
                return userAccount;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Amount has to be greater than zero");
                return Withdraw(userAccount);
            }

            if (userAccount.Balance < amount)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Insufficient balance");
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
                return Withdraw(userAccount);
            }

            userAccount.Balance -= amount;
            Console.WriteLine($"Success. New balance: {userAccount.Balance}");

            string accountJson = File.ReadAllText(@"../../../../files/account.json");
            List<Account> accounts = JsonSerializer.Deserialize<List<Account>>(accountJson);


            var accountToUpdate = accounts.Find(account => account.AccId == userAccount.AccId);
            if (accountToUpdate != null)
            {
                accountToUpdate.Balance = userAccount.Balance;
                string updatedJson = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(@"../../../../files/account.json", updatedJson);
                Console.Clear();
                Console.WriteLine("Withdrawal in process...");
                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("Could not find account");
            }
            return userAccount;
        }

        // money deposit
        public Account Deposit(Account userAccount)
        {
            Console.WriteLine("Deposit in process");

            if (userAccount == null)
            {
                Console.WriteLine("Account not found");
                return null;
            }

            Console.WriteLine($"Current balance: {userAccount.Balance}");
            Console.WriteLine("Deposit amount: ");
            int amount;
            try
            {
                amount = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Enter a valid number, and not a letter.");
                System.Threading.Thread.Sleep(1000);
                return userAccount;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Amount has to be greater than zero");
                return Withdraw(userAccount);
            }

            userAccount.Balance += amount;
            Console.WriteLine($"Success. New balance: {userAccount.Balance}");

            string accountJson = File.ReadAllText(@"../../../../files/account.json");
            List<Account> accounts = JsonSerializer.Deserialize<List<Account>>(accountJson);

            var accountToUpdate = accounts.Find(account => account.AccId == userAccount.AccId);
            if (accountToUpdate != null)
            {
                accountToUpdate.Balance = userAccount.Balance;
                string updatedJson = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(@"../../../../files/account.json", updatedJson);
                Console.Clear();
                Console.WriteLine("Deposit in process...");
                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("Could not find account");
            }
            return userAccount;
        }

        // uppercase name and lastname
        public string Cap(string x)
        {
            return x.Substring(0, 1).ToUpper() + x.Substring(1).ToLower();
        }

        // new acc generation
        public (User, Account) CreateAccount()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            // info prompt
            Console.Clear();
            Console.WriteLine("Preparing for new account creation...");
            System.Threading.Thread.Sleep(1000);
            Console.Clear();

            // user data input
            Console.WriteLine("Enter your first name:");
            string name = Console.ReadLine();
            name = Cap(name);
            Console.WriteLine("Enter your last name:");
            string lastName = Console.ReadLine();
            lastName = Cap(lastName);
            Console.WriteLine("Enter your government ID:");
            Console.WriteLine(name + " " + lastName);
            long govtId;

            // govId format checker
            while (true)
            {
                string input = Console.ReadLine();

                if (long.TryParse(input, out govtId) && input.Length == 11)
                {
                    break;
                }

                Console.WriteLine("Please enter a valid, 11 digit government ID: ");
            }

            Console.WriteLine("Enter your date of birth (YYYY-DD-MM):");
            string dob;
            DateTime correctDate;

            // dob format checker
            while(true)
            {
                dob = Console.ReadLine();

                if (DateTime.TryParseExact(dob, "yyyy-dd-MM", null, System.Globalization.DateTimeStyles.None, out correctDate))
                {
                    break;
                }

                Console.WriteLine("Please enter the date in the correct format.");
            }

            string login;

            while (true)
            {
                Console.WriteLine("Enter your login:");
                login = Console.ReadLine();

                // Load existing users
                string userJson1 = File.Exists(@"../../../../files/user.json") ? File.ReadAllText(@"../../../../files/user.json") : "[]";
                List<User> users1 = JsonSerializer.Deserialize<List<User>>(userJson1, options) ?? new List<User>();

                if (!users1.Any(u => u.UserLogin == login))
                {
                    break; // Login is unique
                }

                Console.WriteLine("This login is already taken. Please choose a different one.");
            }

            Console.WriteLine("Create a password:");
            string password = Console.ReadLine();

            // generating a UserId
            string userJson = File.Exists(@"../../../../files/user.json") ? File.ReadAllText(@"../../../../files/user.json") : "[]";
            List<User> users = JsonSerializer.Deserialize<List<User>>(userJson, options) ?? new List<User>();
            int newUserId = users.Any() ? users.Max(u => u.UserId) + 1 : 1;

            User newUser = new User
            {
                UserId = newUserId,
                UserGovtId = govtId,
                Name = name,
                LastName = lastName,
                DOB = dob,
                UserLogin = login,
                Password = password,
                Status = "Active"
            };

            users.Add(newUser);
            string updatedUserJson = JsonSerializer.Serialize(users, options);
            File.WriteAllText(@"../../../../files/user.json", updatedUserJson);

            // generating acc ID
            string accountJson = File.Exists(@"../../../../files/account.json") ? File.ReadAllText(@"../../../../files/account.json") : "[]";
            List<Account> accounts = JsonSerializer.Deserialize<List<Account>>(accountJson, options) ?? new List<Account>();
            double newAccId = accounts.Any() ? accounts.Max(a => a.AccId) + 1 : 1000;

            Account newAccount = new Account
            {
                AccId = newAccId,
                AccStatus = "Active",
                Balance = 0,
                UserId = newUser.UserId
            };

            // add new acc
            accounts.Add(newAccount);
            string updatedAccountJson = JsonSerializer.Serialize(accounts, options);
            File.WriteAllText(@"../../../../files/account.json", updatedAccountJson);

            Console.Clear();
            Console.WriteLine("Account created successfully.");
            Console.WriteLine($"User ID: {newUserId}");
            Console.WriteLine($"Account ID: {newAccId}");

            return (newUser, newAccount);
        }


        // for testing purposes
        public User(int userId, long userGovtId, string userlogin, string name, string lastName, string dob, string status, string password)
        {
            UserId = userId;
            UserGovtId = userGovtId;
            UserLogin = userlogin;
            Name = name;
            LastName = lastName;
            DOB = dob;
            Status = status;
            Password = password;
        }
    }
}
