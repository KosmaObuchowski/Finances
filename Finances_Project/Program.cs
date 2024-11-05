using System.Reflection.Metadata;
using System.Security.Principal;

namespace Domain.Entities;

class Program
{

    public void Menu()
    {
        Console.WriteLine("BANKING SYSTEM");

        Console.WriteLine("Select an option: ");
        Console.WriteLine("1. Balance Check");
        Console.WriteLine("2. Withdraw");
        Console.WriteLine("3. Deposit");
        Console.WriteLine("4. Check Personal Information");
        Console.WriteLine("5. Logout");

        Console.WriteLine("Selection: ");
    }

    // simulated login system
    public void Login()
    {
        User user = new User();

        Console.WriteLine("Login: ");
        string login = Console.ReadLine();
        if (login != null) {
            Console.WriteLine("Password: ");
            string password = Console.ReadLine();
            if (password == user.Password) //!string.IsNullOrEmpty(password)
            {
                Console.WriteLine("Welcome " + user.Name);
            }
            else
            {
                Console.WriteLine("Incorrect password");
            }
        }
    }

    static void Main()
    {


        Console.Title = "Finance App";

        User user = new User();
        Console.WriteLine(user.Name);

        var login = new Program();
        login.Login();

        bool logout = false;

        while (!logout)
        {
            
            var menu = new Program();
            menu.Menu();

            try
            {
                int option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 4:
                        
                        break;

                    case 5:
                        Console.WriteLine(user.Name + " is now logged out.");
                        System.Threading.Thread.Sleep(3000);
                        return;

                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Please select a number in range.");
            }


            


        }

        
    }
}

// add goals