using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                default:
                    System.Console.WriteLine("Invalid Option.");
                    break;
            }
        }
    }

}
