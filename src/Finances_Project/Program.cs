using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text.Json;

namespace Domain.Entities;

class Program
{
    public void Correction()
    {
        Console.Clear();
        Console.WriteLine("Please select a number in range, and not a letter.");
        System.Threading.Thread.Sleep(2000);
    }

    public void Menu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Clear();
        Console.WriteLine("BANKING SYSTEM");

        Console.WriteLine("Select an option: ");
        Console.WriteLine("1. Balance Check");
        Console.WriteLine("2. Withdraw");
        Console.WriteLine("3. Deposit");
        Console.WriteLine("4. Check Personal Information");
        Console.WriteLine("5. Display log file");
        Console.WriteLine("6. Logout");

        Console.WriteLine("Selection: ");
    }

    static void Main()
    {
        Console.Title = "Finance App";

        Console.ForegroundColor = ConsoleColor.Cyan;

        var correction = new Program();
        var menu = new Program();

        Console.WriteLine("Log in or sign up to the banking system: 1. Log in, 2. Sign up.");

        User user = new User();

        bool logout = false;
        while (!logout)
        {
            try
            {
                int option = Convert.ToInt32(Console.ReadLine());
                if (option < 1 || option > 2)
                {
                    correction.Correction();
                }
                else if (option == 1)
                {
                    logout = true;
                    Console.Clear();
                }
                else if (option == 2)
                {
                    (User newUser, Account newAccount) = user.CreateAccount();

                    Console.Clear();
                    Console.WriteLine("Press any key to log in with your new account.");
                    Console.ReadKey();

                    logout = true;
                }
            }
            catch (FormatException)
            {
                correction.Correction();
            }
        }

        (User currentUser, Account currentAccount) = user.Login();

        logout = false;
        while (!logout)
        {
            menu.Menu();

            try
            {
                int option = Convert.ToInt32(Console.ReadLine());

                if (option <= 0 || option > 6)
                {
                    correction.Correction();
                }
                else
                {
                    switch (option)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine($"Account ID: {currentAccount.AccId}");
                            Console.WriteLine($"Account Status: {currentAccount.AccStatus}");
                            Console.WriteLine($"Account Balance: {currentAccount.Balance}");
                            Console.WriteLine("Press any key to continue... ");
                            Console.ReadKey();
                            break;
                        case 2:
                            Console.Clear();
                            currentAccount = user.Withdraw(currentAccount);
                            break;
                        case 3:
                            Console.Clear();
                            currentAccount = user.Deposit(currentAccount);
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("Name: " + currentUser.Name + "\nLast Name: " + currentUser.LastName + "\nGovernment ID: " + currentUser.UserGovtId + "\nDate of Birth: " + currentUser.DOB);
                            Console.WriteLine($"User status: {currentAccount.AccStatus}");
                            Console.WriteLine("Press any key to continue... ");
                            Console.ReadKey();
                            break;
                        case 5:
                            if (currentUser.UserGovtId == 12345678911)
                            {
                                string path = @"../../../../files/log.txt";

                                if (File.Exists(path))
                                {
                                    string[] lines = File.ReadAllLines(path);
                                    foreach (string line in lines)
                                    {
                                        Console.WriteLine(line);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("File does not exist.");
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Unathorized access.");
                            }
                            Console.WriteLine("Press any key to continue... ");
                            Console.ReadKey();
                            break;
                        case 6:
                            Console.Clear();
                            Console.WriteLine($"User {currentUser.Name} {currentUser.LastName} has been successfully logged out.");
                            logout = true;
                            System.Threading.Thread.Sleep(2222);
                            break;
                    }
                }
            }
            catch (FormatException)
            {
                correction.Correction();
            }

        }

        string filePath = @"../../../../files/log.txt";
        File.AppendAllText(filePath, $"\n{DateTime.Now} User {currentUser.Name} {currentUser.LastName} has been successfully logged out.");
    }
}
