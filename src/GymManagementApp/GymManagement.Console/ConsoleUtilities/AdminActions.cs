using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Infrastructure;

namespace GymManagement.Console.ConsoleUtilities
{
    public static class AdminActions
    {
        public static async Task HandleAdminActionsAsync(string input)
        {
            switch (input)
            {
                case "1":
                    await MembershipActions.AddMemberShipAsync();
                    break;
                case "2":
                    await MemberActions.AddMemberAsync();
                    break;
                case "3":
                    await TrainerActions.AddTrainerAsync();
                    break;
                case "4":
                    await MemberActions.PrintBookingsAsync();
                    break;
                case "5":
                    await MemberActions.PrintMembersAsync();
                    break;
                case "6":
                    await TrainerActions.PrintTrainersAsync();
                    break;
                case "7":
                    await MembershipActions.PrintMemberShipsAsync();
                    break;
                case "8":
                    await MemberActions.DeleteMemberAsync();
                    break;
                case "9":
                    await FindMemberByEmail();
                    break;
                case "10":
                    await InitializeDatabaseAsync();
                    break;

                default:
                    System.Console.WriteLine("Invalid Option.");
                    break;
            }
        }


        private static async Task InitializeDatabaseAsync()
        {
            try
            {
                System.Console.WriteLine("Initializing database...");

                using (var uow = new UnitOfWork())
                {
                    
                    string dbPath = uow.GetDbPath();
                    System.Console.WriteLine($"Database Path: {dbPath}");

                    
                    await uow.ApplyMigrationsAsync();

                    System.Console.WriteLine("Database initialized successfully.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error during database initialization: {ex.Message}");
            }
        }




        private static async Task FindMemberByEmail()
        {
            using (var uow = new UnitOfWork())
            {
                var input = System.Console.ReadLine();
                var member = await uow.Members.GetMemberByEmailAsync(input);
                if (member == null)
                {
                    System.Console.WriteLine("There's no member with this email yet");
                }
                else
                {
                    System.Console.WriteLine($"Here's information about your member\n" +
                 $"Full name: {member.FullName},\n" +
                 $"Username: {member.Username}\n" +
                 $"Phone Number: {member.PhoneNumber}");
                }
            }
        }
    }
}
