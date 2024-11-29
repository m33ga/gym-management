using GymManagement.Domain.Models;
using GymManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.Console.ConsoleUtilities
{
    public static class MemberActions
    {
        public static async Task HandleMemberActionsAsync(string input)
        {
            switch (input)
            {
                case "1":
                    await MemberActions.AddBookingAsync();
                    break;
                case "2":
                    await MembershipActions.AddMemberShipAsync();
                    break;
                case "3":
                    await PrintBookingsAsync();
                    break;
                case "4":
                    await DeleteMemberAsync();
                    break;
                default:
                    System.Console.WriteLine("Invalid Option.");
                    break;
            }
        }

        public static async Task AddMemberAsync()
        {
            using (var uow = new UnitOfWork())
            {
                Membership mb1 = new("Test", 150, "Test Description", 15, 50);
                Member m1 = new("TestName", "test@gmail.com", "password", "testuser1", "+123456789", 82.1F, 1.91F, 15, mb1);
                await uow.Members.AddMemberAsync(m1);
                await uow.SaveChangesAsync();
            }
        }

        public static async Task PrintMembersAsync()
        {
            using (var uow = new UnitOfWork())
            {
                System.Console.WriteLine($"Database path: {uow.GetDbPath()}");

                var list = await uow.Members.FindAllAsync();
                if (list.Count == 0)
                {
                    System.Console.WriteLine("\n There are no Members yet");
                }
                else
                {
                    System.Console.WriteLine("\n Members:");
                    foreach (var member in list)
                    {
                        System.Console.WriteLine($"Member: {member.Id}, " +
                            $"Member Name: {member.FullName}," +
                            $" Member Email: {member.Email}," +
                            $" Member Username {member.Username}, " +
                            $"Member Phone Number{member.PhoneNumber}," +
                            $" Member Weight: {member.Weight}kg, " +
                            $"Member Height: {member.Height}cm, " +
                            $"Member Reamining workouts: {member.RemainingWorkouts}");

                    }
                }
            }
        }

        public static async Task AddBookingAsync()
        {
            using (var uow = new UnitOfWork())
            {
                DateTime date1 = DateTime.Now.AddDays(1);
                Trainer t1 = new("Test Name", "password", "test@gmail.com", "testuser", "+123456789");

                Membership mb1 = new("Test", 150, "Test Description", 15, 50);

                Member m1 = new("TestName", "test@gmail.com", "password", "testuser1", "+123456789", 82.1F, 1.91F, 15, mb1);

                Class c1 = new("TestName", "TestDescription", date1, 50, t1);

                Booking b1 = new(null, 3, date1);
                if (b1.MemberId == null)
                {
                    b1.Cancel();
                }
                await uow.Bookings.AddBookingAsync(b1);
                await uow.SaveChangesAsync();

            }

        public static async Task PrintBookingsAsync()
        {
            using (var uow = new UnitOfWork())
            {
                System.Console.WriteLine($"Database path: {uow.GetDbPath()}");

                var list = await uow.Bookings.FindAllAsync();
                if (list.Count == 0)
                {
                    System.Console.WriteLine("\n There are no bookings yet");
                }
                else
                {
                    System.Console.WriteLine("\n Bookings:");
                    foreach (var booking in list)
                    {
                        System.Console.WriteLine($"Booking: {booking.BookingDate}, " +
                            $"Class ID: {booking.ClassId}, " +
                            $"Member ID: {booking.MemberId}");

                    }
                }
            }

        }

        public static async Task DeleteMemberAsync()
        {
            using (var uow = new UnitOfWork())
            {
                System.Console.WriteLine("Enter Member ID to delete:");
                if (int.TryParse(System.Console.ReadLine(), out int memberId))
                {
                    var member = await uow.Members.FindByIdAsync(memberId);
                    if (member == null)
                    {
                        System.Console.WriteLine("Member not found.");
                        return;
                    }

                    uow.Members.Delete(member);
                    await uow.SaveChangesAsync();
                    System.Console.WriteLine("Member deleted successfully.");
                }
                else
                {
                    System.Console.WriteLine("Invalid ID entered.");
                }
            }
        }

    }
}
