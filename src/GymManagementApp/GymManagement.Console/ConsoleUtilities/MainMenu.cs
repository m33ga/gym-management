using GymManagement.ConsoleUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.Console.ConsoleUtilities
{
    public static class MainMenu
    {
        public static async Task ShowMainMenuAsync()
        {
            var exit = false;
            do
            {
                System.Console.Clear();
                System.Console.WriteLine("App menu");
                System.Console.WriteLine("--------");

                System.Console.WriteLine("1. Register");
                System.Console.WriteLine("2. Login");
                System.Console.WriteLine("0. Exit");

                var option = System.Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await UserActions.RegisterAsync();
                        break;

                    case "2":
                        var role = await UserActions.LoginAsync();
                        if (role != null)
                        {
                            // Navigate to role-specific post-login menu
                            await RoleMenus.ShowPostLoginMenuAsync(role);
                        }
                        break;

                    case "0":
                        exit = true;
                        break;

                    default:
                        System.Console.WriteLine("Please enter a valid choice");
                        System.Console.ReadKey();
                        break;
                }
            } while (!exit);
        }
    }
}
