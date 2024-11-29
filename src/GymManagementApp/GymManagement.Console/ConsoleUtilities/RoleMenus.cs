using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.Console.ConsoleUtilities
{
    public static class RoleMenus
    {
        public static async Task ShowPostLoginMenuAsync(string role)
        {
            var exit = false;

            do
            {
                System.Console.Clear();
                System.Console.WriteLine($"====== {role} Menu ======");

                switch (role)
                {
                    case "Admin":
                        System.Console.WriteLine("1. Create Membership");
                        System.Console.WriteLine("2. Create Member");
                        System.Console.WriteLine("3. Create Trainer");
                        System.Console.WriteLine("4. Print Bookings");
                        System.Console.WriteLine("5. Print Members");
                        System.Console.WriteLine("6. Print Trainers");
                        System.Console.WriteLine("7. Print Memberships");
                        System.Console.WriteLine("8. Delete Member");
                        System.Console.WriteLine("0. Logout");
                        break;

                    case "Trainer":
                        System.Console.WriteLine("1. Create Class");
                        System.Console.WriteLine("2. Print Upcoming Classes(booked by someone) for each Trainer");
                        System.Console.WriteLine("3. Print All Classes for each Trainer");
                        System.Console.WriteLine("0. Logout");
                        break;

                    case "Member":
                        System.Console.WriteLine("1. Create Booking");
                        System.Console.WriteLine("2. Create Membership");
                        System.Console.WriteLine("3. Print Bookings");
                        System.Console.WriteLine("4. Delete Member");
                        System.Console.WriteLine("5. Update Member (not yet available)");
                        System.Console.WriteLine("6. Create Unbooked Booking (Unhandled exception)"); // handle exception
                        System.Console.WriteLine("7. Print Upcoming Classes for each member ");
                        System.Console.WriteLine("8. Print All Available Classes");
                        System.Console.WriteLine("0. Logout");
                        break;
                }

                var input = System.Console.ReadLine();

                switch (role)
                {
                    case "Admin":
                        await AdminActions.HandleAdminActionsAsync(input);
                        break;

                    case "Trainer":
                        await TrainerActions.HandleTrainerActionsAsync(input);
                        break;

                    case "Member":
                        await MemberActions.HandleMemberActionsAsync(input);
                        break;
                }

                if (input == "0")
                {
                    exit = true; // Logout
                }

                System.Console.WriteLine("Press any key to continue");
                System.Console.ReadKey();

            } while (!exit);
        }
    }
}
