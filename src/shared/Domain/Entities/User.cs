using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;
using System.IO;

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

            //string userJson = File.ReadAllText(@"./user.json");
            string userJson = File.ReadAllText(@"../../../../files/user.json");
            List<User> user = JsonSerializer.Deserialize<List<User>>(userJson, options);
            //Console.WriteLine($"TEST Name: {user[0].Name} TEST");

            //var path = Directory.GetCurrentDirectory();
            //Console.WriteLine(path);

            if (user == null)
            {
                Console.WriteLine("User does not exist.");
                // method to create another bank account
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
                //Login();
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
                //return userAccount;
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

        public (User, Account) CreateAccount()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            Console.WriteLine("Creating a new account...");
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine("Enter your first name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter your last name:");
            string lastName = Console.ReadLine();
            Console.WriteLine("Enter your government ID:");
            long govtId;
            while (!long.TryParse(Console.ReadLine(), out govtId))
            {
                Console.WriteLine("Invalid input. Please enter a valid government ID:");
            }

            Console.WriteLine("Enter your date of birth (YYYY-DD-MM):");
            string dob = Console.ReadLine();

            Console.WriteLine("Enter your login:");
            string login = Console.ReadLine();

            Console.WriteLine("Create a password:");
            string password = Console.ReadLine();

            // Generate a unique UserId (based on the existing JSON file)
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

            // Generate a unique Account ID
            string accountJson = File.Exists(@"../../../../files/account.json") ? File.ReadAllText(@"../../../../files/account.json") : "[]";
            List<Account> accounts = JsonSerializer.Deserialize<List<Account>>(accountJson, options) ?? new List<Account>();
            double newAccId = accounts.Any() ? accounts.Max(a => a.AccId) + 1 : 1000;

            Account newAccount = new Account
            {
                AccId = newAccId,
                AccStatus = "Active",
                Balance = 0,
                UserId = newUser.UserId // Link the account to the user
            };

            // Add the new account to the list and save it to the JSON file
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