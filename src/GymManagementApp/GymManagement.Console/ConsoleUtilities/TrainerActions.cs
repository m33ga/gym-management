using GymManagement.Domain.Models;
using GymManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.Console.ConsoleUtilities
{
    public static class TrainerActions
    {
        public static async Task HandleTrainerActionsAsync(string input)
        {
            switch (input)
            {
                case "1":
                    await ClassActions.AddClassAsync();
                    break;
                // Add PrintTrainerClassesAsync when implemented
                default:
                    System.Console.WriteLine("Invalid Option.");
                    break;
            }
        }

        public static async Task AddTrainerAsync()
        {
            using (var uow = new UnitOfWork())
            {
                Trainer t1 = new("Test Name", "password", "test@gmail.com", "testuser", "+123456789");
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
                    System.Console.WriteLine("\n There are no Members yet");
                }
                else
                {
                    System.Console.WriteLine("\n Members:");
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
    }
}
