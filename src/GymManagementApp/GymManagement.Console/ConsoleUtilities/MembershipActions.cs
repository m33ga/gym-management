using GymManagement.Domain.Models;
using GymManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.Console.ConsoleUtilities
{
    public static class MembershipActions
    {

        public static async Task<Membership> EnsureDefaultMembershipAsync(UnitOfWork uow)
        {
            // Check if a default membership already exists
            var membership = await uow.Memberships.FindByIdAsync(1);
            if (membership == null)
            {
                System.Console.WriteLine("No memberships found. Creating a default membership.");
                membership = new Membership("Standard", 100, "Default Membership", 10, 30);
                await uow.Memberships.AddMembershipAsync(membership);
                await uow.SaveChangesAsync();
                System.Console.WriteLine("Default membership created successfully.");
            }
            return membership;
        }
        public static async Task AddMemberShipAsync()
        {
            using (var uow = new UnitOfWork())
            {
                Membership mb1 = new("Test", 150, "Test Description", 15, 50);
                await uow.Memberships.AddMembershipAsync(mb1);
                await uow.SaveChangesAsync();
            }
        }

        public static async Task PrintMemberShipsAsync()
        {
            using (var uow = new UnitOfWork())
            {
                System.Console.WriteLine($"Database path: {uow.GetDbPath()}");

                var list = await uow.Memberships.FindAllAsync();
                if (list.Count == 0)
                {
                    System.Console.WriteLine("\n There are no memberships yet");
                }
                else
                {
                    System.Console.WriteLine("\n Memberships:");
                    foreach (var membership in list)
                    {
                        System.Console.WriteLine($"Membership Tier: {membership.Tier}, " +
                            $"Price: {membership.Price}, " +
                            $"Description: {membership.Description}, " +
                            $"Trainings Quantity: {membership.TrainingsQuantity}, " +
                            $"Duration: {membership.Duration}");

                    }
                }
            }

        }
    }
}
