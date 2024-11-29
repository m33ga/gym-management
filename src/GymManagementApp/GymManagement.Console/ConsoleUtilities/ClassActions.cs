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
            System.Console.WriteLine("Adding a random class:");
            using (var uow = new UnitOfWork())
            {
                DateTime date1 = DateTime.Now.AddDays(10).AddHours(10);
                Trainer t1 = await uow.Trainers.FindByIdAsync(1);
                Class c1 = new("TestName", "TestDescription", date1, 60, t1);
                await uow.Classes.AddClassAsync(c1);
                await uow.SaveChangesAsync();
                System.Console.WriteLine("Class added successfully");
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
                    System.Console.WriteLine("\n There are no classes yet");
                }
                else
                {
                    System.Console.WriteLine("\n Classes:");
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
                    System.Console.WriteLine("\n There are no available classes");
                }
                else
                {
                    foreach (var classi in list)
                    {
                        System.Console.WriteLine($"class ID: {classi.Id}, " +
                                                 $"class Name: {classi.Name}, " +
                                                 $"class Description: {classi.Description}, " +
                                                 $"class Date: {classi.ScheduledDate}, " +
                                                 $"class Duration: {classi.DurationInMinutes}min, " +
                                                 $"class Trainer ID: {classi.TrainerId}, " +
                                                 $"class Availability: {classi.IsAvailable}, ");
                    }
                }
            }
        }
    }
}
