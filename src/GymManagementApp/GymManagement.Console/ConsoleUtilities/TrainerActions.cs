using GymManagement.Domain.Models;
using GymManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.ConsoleUtilities;

namespace GymManagement.Console.ConsoleUtilities
{
    public static class TrainerActions
    {
        public static async Task<bool> HandleTrainerActionsAsync(string input)
        {
            switch (input)
            {
                case "1":
                    await ClassActions.AddClassAsync();
                    break;
                case "2":
                    await PrintAllBookedClassesByTrainer();
                    break;
                case "3":
                    await PrintClassesByTrainer();
                    break;
                default:
                    System.Console.WriteLine("Invalid Option.");
                    break;
            }
            return false;
        }

        public static async Task AddTrainerAsync()
        {
            using (var uow = new UnitOfWork())
            {
                Trainer t1 = new("Test Name", "password", "test@gmail.com", "testuser", "+123456789");

                //System.Console.WriteLine("Enter Trainer Name:");
                //string name = System.Console.ReadLine();
                //System.Console.WriteLine("Enter Trainer Email:");
                //string email = System.Console.ReadLine();
                //System.Console.WriteLine("Enter Trainer Username:");
                //string username = System.Console.ReadLine();
                //System.Console.WriteLine("Enter Trainer Password:");
                //string password = System.Console.ReadLine();
                //System.Console.WriteLine("Enter Trainer Phone Number:");
                //string phoneNumber = System.Console.ReadLine();
                //Trainer tr = new(name, PasswordUtils.HashPassword(password), email, username, phoneNumber);
                await uow.Trainers.AddTrainerAsync(t1);
                await uow.SaveChangesAsync();
            }
        }

        public static async Task PrintTrainersAsync()
        {
            using (var uow = new UnitOfWork())
            {
                System.Console.WriteLine($"Database path: {uow.GetDbPath()}");

                var list = await uow.Trainers.FindAllAsync();
                if (list.Count == 0)
                {
                    System.Console.WriteLine("\n There are no Trainers yet");
                }
                else
                {
                    System.Console.WriteLine("\n Trainer:");
                    foreach (var trainer in list)
                    {
                        System.Console.WriteLine($"Trainer: {trainer.Id}, " +
                            $"Trainer Name: {trainer.FullName}," +
                            $" Trainer Email: {trainer.Email}," +
                            $" Trainer Username: {trainer.Username}, " +
                            $"Trainer Phone Number: {trainer.PhoneNumber}," +
                            $" Trainer Image: {trainer.Image}, ");


                    }
                }
            }
        }

        public static async Task PrintAllBookedClassesByTrainer()
        {
            using (var uow = new UnitOfWork())
            {
                var trainers = await uow.Trainers.FindAllAsync();
                foreach (var trainer in trainers)
                {
                    var classes = await uow.Classes.GetBookedClassesByTrainerAsync(trainer.Id);
                    System.Console.WriteLine($"Trainer: {trainer.FullName}");
                    foreach (var c in classes)
                    {
                        System.Console.WriteLine($"Class: {c.Name}, Date: {c.ScheduledDate}, Is Available:{c.IsAvailable}");
                    }
                }
            }
        }

        public static async Task PrintClassesByTrainer()
        {
            using (var uow = new UnitOfWork())
            {
                var trainers = await uow.Trainers.FindAllAsync();
                foreach (var trainer in trainers)
                {
                    var classes = await uow.Classes.GetClassesByTrainerAsync(trainer.Id);
                    System.Console.WriteLine($"Trainer: {trainer.FullName}");
                    foreach (var c in classes)
                    {
                        System.Console.WriteLine($"Class: {c.Name}, Date: {c.ScheduledDate}, Is Available: {c.IsAvailable}");
                    }
                }
            }
        }


    }
}
