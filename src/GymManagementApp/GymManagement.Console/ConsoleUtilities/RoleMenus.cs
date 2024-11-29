using System;
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
                        System.Console.WriteLine("9. Search Member by Email");
                        System.Console.WriteLine("10. Initialize Database");
                        System.Console.WriteLine("0. Logout");
                        break;

                    case "Trainer":
                        System.Console.WriteLine("1. Create Class");
                        System.Console.WriteLine("2. Print Upcoming Classes (booked)");
                        System.Console.WriteLine("3. Print All Classes");
                        System.Console.WriteLine("0. Logout");
                        break;

                    case "Member":
                        System.Console.WriteLine("1. Create Booking");
                        System.Console.WriteLine("2. Create Membership");
                        System.Console.WriteLine("3. Print Bookings");
                        System.Console.WriteLine("4. Delete Account");
                        System.Console.WriteLine("5. Update Account");
                        System.Console.WriteLine("6. Create Unbooked Booking (Unhandled exception)"); // Handle exception
                        System.Console.WriteLine("7. Find Available Classes");
                        System.Console.WriteLine("8. Print Upcoming Classes for Member");
                        System.Console.WriteLine("9. Print Trainer Rating");
                        System.Console.WriteLine("10. Create review for class");
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
                        bool shouldExit = await MemberActions.HandleMemberActionsAsync(input);
                        if (shouldExit || input == "0")
                        {
                            exit = true; // Exit the loop
                        }
                        break;

                    default:
                        System.Console.WriteLine("Invalid role.");
                        break;
                }

                if (!exit)
                {
                    System.Console.WriteLine("Press any key to continue...");
                    System.Console.ReadKey();
                }

            } while (!exit);
        }
    }
}
