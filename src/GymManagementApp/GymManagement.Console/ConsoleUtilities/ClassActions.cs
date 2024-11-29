using GymManagement.Domain.Models;
using GymManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.Console.ConsoleUtilities
{
    public static class ClassActions
    {
        public static async Task AddClassAsync()
        {
            using (var uow = new UnitOfWork())
            {
                DateTime date1 = new(2025, 11, 26);
                Trainer t1 = new("Test Name", "password", "test@gmail.com", "testuser", "+123456789");
                Class c1 = new("TestName", "TestDescription", date1, 50, t1);
                await uow.Classes.AddClassAsync(c1);
                await uow.SaveChangesAsync();
            }

        }

        public static async Task PrintClassesAsync()
        {
            using (var uow = new UnitOfWork())
            {
                System.Console.WriteLine($"Database path: {uow.GetDbPath()}");

                var list = await uow.Classes.FindAllAsync();
                if (list.Count == 0)
                {
                    System.Console.WriteLine("\n There are no Members yet");
                }
                else
                {
                    System.Console.WriteLine("\n Members:");
                    foreach (var classi in list)
                    {
                        System.Console.WriteLine($"class: {classi.Id}, " +
                            $"class Name: {classi.Name}, " +
                            $"class Description: {classi.Description}, " +
                            $"class Date: {classi.ScheduledDate}, " +
                            $"class Duration: {classi.DurationInMinutes}min, " +
                            $"class Trainer ID: {classi.TrainerId}, " +
                            $"Class Availability: {classi.IsAvailable}, " +
                            $"Class Member ID: {classi.MemberId} ");
                    }
                }
            }
        }

        public static async Task PrintAvailableClasses()
        {
            using (var uow = new UnitOfWork())
            {
                var list = await uow.Classes.GetAvailableClassesAsync();
                if (list.Count == 0)
                {
                    System.Console.WriteLine("\n There are no available classes yet");
                }
                else
                {
                    System.Console.WriteLine("\n Members:");
                    foreach (var classi in list)
                    {
                        System.Console.WriteLine($"class: {classi.Id}, " +
                                                 $"class Name: {classi.Name}, " +
                                                 $"class Description: {classi.Description}, " +
                                                 $"class Date: {classi.ScheduledDate}, " +
                                                 $"class Duration: {classi.DurationInMinutes}min, " +
                                                 $"class Trainer ID: {classi.TrainerId}, " +
                                                 $"Class Availability: {classi.IsAvailable}, ");
                    }
                }
            }
        }
    }
}
