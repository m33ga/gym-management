﻿using GymManagement.ConsoleUtilities;
using GymManagement.Domain.Models;
using GymManagement.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
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
                    await AddBookingAsync();
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

                case "5":
                    await UpdateMemberAsync();
                    break;
                case "6":
                    await AddUnbookedBookingAsync();
                    break;
                case "7":
                    await FindAvailableClasses();
                    break;
                case "8":
                    await PrintUpcomingClassesForAllMembers();
                    break;
                default:
                    System.Console.WriteLine("Invalid Option.");
                    break;
            }
        }

        private static async Task FindAvailableClasses()
        {
            using (var uow = new UnitOfWork())
            {
                System.Console.WriteLine($"Database path: {uow.GetDbPath()}");

                var list = await uow.Classes.GetAvailableClassesAsync();
           
                if (list.Count == 0)
                {
                    System.Console.WriteLine("\n There are no available classes");
                }
                else
                {
                    System.Console.WriteLine("\n Classes:");
                    foreach (var AvailableClass in list)
                    {
                        var t = AvailableClass.TrainerId;
                        var tpr = await uow.Trainers.GetByIdWithDetailsAsync(t);
                        System.Console.WriteLine($"Class: {AvailableClass.Name}\n" +
                            $"Description: {AvailableClass.Description}\n" +
                            $"ScheduledDate: {AvailableClass.ScheduledDate}\n" +
                            $"Duration: {AvailableClass.DurationInMinutes}min\n" +
                            $"Trainer: {tpr.FullName}, {tpr.Email}\n");
                    }
                }
            }
        }
        

        private static async Task AddUnbookedBookingAsync() //wtf is this for?
        {
            using (var uow = new UnitOfWork())
            {
                DateTime date1 = DateTime.Now.AddDays(1);
                Trainer t1 = new("Test Name", "password", "test@gmail.com", "testuser", "+123456789");

                Membership mb1 = new("Test", 150, "Test Description", 15, 50);

                Member m1 = new("TestName", "test@gmail.com", "password", "testuser1", "+123456789", 82.1F, 1.91F, 15, mb1);

                Class c1 = new("TestName", "TestDescription", date1, 50, t1);

                Booking b1 = new(null, 2, date1);
                if (b1.MemberId == null)
                {
                    b1.Cancel();
                }
                await uow.Bookings.AddBookingAsync(b1);
                await uow.SaveChangesAsync();
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
                            $"Member Remaining workouts: {member.RemainingWorkouts}");
                    }
                }
            }
        }

        public static async Task AddBookingAsync()
        {
            using (var uow = new UnitOfWork())
            {
                //DateTime date1 = DateTime.Now.AddDays(1);
                //Trainer t1 = new("Test Name", "password", "test@gmail.com", "testuser", "+123456789");

                //Membership mb1 = new("Test", 150, "Test Description", 15, 50);

                //Member m1 = new("TestName", "test@gmail.com", "password", "testuser1", "+123456789", 82.1F, 1.91F, 15, mb1);

                //Class c1 = new("TestName", "TestDescription", date1, 50, t1);

                //Booking b1 = new(1, 1, date1);
                //if (b1.MemberId == null)
                //{
                //    b1.Cancel();
                //}

                System.Console.WriteLine("Available Classes:");
                await ClassActions.PrintAvailableClasses();
                System.Console.WriteLine("Enter Class ID to book:");
                if (int.TryParse(System.Console.ReadLine(), out int classId))
                {
                    var c = await uow.Classes.FindByIdAsync(classId);
                    if (c == null)
                    {
                        System.Console.WriteLine("Class not found.");
                        return;
                    }

                    System.Console.WriteLine("Member List:");
                    await PrintMembersAsync();
                    System.Console.WriteLine("Enter Member ID who is booking:");
                    if (int.TryParse(System.Console.ReadLine(), out int memberId))
                    {
                        var m = await uow.Members.FindByIdAsync(memberId);
                        if (m == null)
                        {
                            System.Console.WriteLine("Member not found.");
                            return;
                        }

                        Booking b = new Booking(memberId, classId, DateTime.Now);
                        await uow.Bookings.AddBookingAsync(b);
                        await uow.SaveChangesAsync();
                        System.Console.WriteLine("Booking added successfully.");
                    }
                    else
                    {
                        System.Console.WriteLine("Invalid Member ID entered.");
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid Class ID entered.");
                }
            }
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

        public static async Task UpdateMemberAsync()
        {
            using (var uow = new UnitOfWork())
            {
                System.Console.WriteLine("Enter Member ID to update:");
                if (int.TryParse(System.Console.ReadLine(), out int memberId))
                {
                    var member = await uow.Members.FindByIdAsync(memberId);
                    if (member == null)
                    {
                        System.Console.WriteLine("Member not found.");
                        return;
                    }

                    System.Console.WriteLine("Enter new height (in cm):");
                    if (!float.TryParse(System.Console.ReadLine(), out float newHeight))
                    {
                        System.Console.WriteLine("Invalid height value entered.");
                        return;
                    }

                    System.Console.WriteLine("Enter new weight (in kg):");
                    if (!float.TryParse(System.Console.ReadLine(), out float newWeight))
                    {
                        System.Console.WriteLine("Invalid weight value entered.");
                        return;
                    }

                    System.Console.WriteLine("Do you want to update the password? (yes/no):");
                    string updatePasswordResponse = System.Console.ReadLine()?.Trim().ToLower();

                    if (updatePasswordResponse == "yes")
                    {
                        System.Console.WriteLine("Enter new password:");
                        string newPassword = System.Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                        {
                            System.Console.WriteLine("Password must be at least 6 characters long.");
                            return;
                        }

                        // Hash the new password
                        string hashedPassword = PasswordUtils.HashPassword(newPassword);

                        // Update the member's password
                        member.UpdatePassword(hashedPassword);
                        System.Console.WriteLine("Password updated successfully.");
                    }

                    // Update member's height and weight
                    member.UpdateProfile(member.FullName, member.PhoneNumber, newWeight, newHeight, member.Image);

                    await uow.SaveChangesAsync();
                    System.Console.WriteLine("Member's profile updated successfully.");
                }
                else
                {
                    System.Console.WriteLine("Invalid ID entered.");
                }
            }
        }

        public static async Task PrintUpcomingClassesForAllMembers()
        {
            using (var uow = new UnitOfWork())
            {
                var members = await uow.Members.FindAllAsync();
                foreach (var member in members)
                {
                    var classes = await uow.Bookings.GetClassesByMemberAsync(member.Id);
                    System.Console.WriteLine($"Member: {member.FullName}");
                    foreach (var c in classes)
                    {
                        System.Console.WriteLine($"Class: {c.Name}, Date: {c.ScheduledDate}");
                    }
                }
            }
        }

    }


}
