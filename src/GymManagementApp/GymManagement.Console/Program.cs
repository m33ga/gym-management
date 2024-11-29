using System;
using GymManagement.Infrastructure;
using GymManagement.Domain;
using GymManagement.Application;
using GymManagement.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
Console.WriteLine("Testing GymManagement");
var exit = false;

do
{
    Console.Clear();
    Console.WriteLine("App menu");
    Console.WriteLine("--------");
    
    Console.WriteLine("1. Create Booking");
    Console.WriteLine("2. Create Member");
    Console.WriteLine("3. Create Trainer");
    Console.WriteLine("4. Create Class");
    Console.WriteLine("5. Create Membership");
    Console.WriteLine("6. Print Bookings");
    Console.WriteLine("7. Print Members");
    Console.WriteLine("8. Print Trainers");
    Console.WriteLine("9. Print Classes");
    Console.WriteLine("a. Print Memberships");
    Console.WriteLine("z. Print Upcoming Classes for each Member");
    Console.WriteLine("y. Print All Classes for each Trainer");
    Console.WriteLine("x. Print Booked Classes for each Trainer");
    Console.WriteLine("0. exit");
    var option = Console.ReadLine();
    switch (option)
    {
        case "1":
            await AddBookingAsync();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;

        case "2":
            await AddMemberAsync();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break ;
        case "3":
            await AddTrainerAsync();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case "4":
            await AddClassAsync();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case "5":
            await AddMemberShipAsync();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case "6":
            await PrintBookingsAsync();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case "7":
            await PrintMembersAsync();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case "8":
            await PrintTrainersAsync();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case "9":
            await PrintClassesAsync();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case "a":
            await PrintMemberShipsAsync();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case "z":
            await PrintUpcomingClassesForAllMembers();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case "y":
            await PrintClassesByTrainer();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case "x":
            await PrintAllBookedClassesByTrainer();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case "0":
            exit = true;
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;

        default:
            Console.WriteLine("Please enter a valid choice");
            Console.ReadKey();
            break;
    }
} while (!exit);

async Task PrintMemberShipsAsync()
{
    using (var uow = new UnitOfWork())
    {
        Console.WriteLine($"Database path: {uow.GetDbPath()}");

        var list = await uow.Memberships.FindAllAsync();
        if (list.Count == 0)
        {
            Console.WriteLine("\n There are no memberships yet");
        }
        else
        {
            Console.WriteLine("\n Memberships:");
            foreach (var membership in list)
            {
                Console.WriteLine($"Membership Tier: {membership.Tier}, " +
                    $"Price: {membership.Price}, " +
                    $"Description: {membership.Description}, " +
                    $"Trainings Quantity: {membership.TrainingsQuantity}, " +
                    $"Duration: {membership.Duration}");

            }
        }
    }

}
async Task PrintClassesAsync()
{
    using (var uow = new UnitOfWork())
    {
        Console.WriteLine($"Database path: {uow.GetDbPath()}");

        var list = await uow.Classes.FindAllAsync();
        if (list.Count == 0)
        {
            Console.WriteLine("\n There are no Members yet");
        }
        else
        {
            Console.WriteLine("\n Members:");
            foreach (var classi in list)
            {
                Console.WriteLine($"class: {classi.Id}, " +
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
async Task PrintTrainersAsync()
{
    using (var uow = new UnitOfWork())
    {
        Console.WriteLine($"Database path: {uow.GetDbPath()}");

        var list = await uow.Trainers.FindAllAsync();
        if (list.Count == 0)
        {
            Console.WriteLine("\n There are no Members yet");
        }
        else
        {
            Console.WriteLine("\n Members:");
            foreach (var trainer in list)
            {
                Console.WriteLine($"Trainer: {trainer.Id}, " +
                    $"Trainer Name: {trainer.FullName}," +
                    $" Trainer Email: {trainer.Email}," +
                    $" Trainer Username: {trainer.Username}, " +
                    $"Trainer Phone Number: {trainer.PhoneNumber}," +
                    $" Trainer Image: {trainer.Image}, ");
                    

            }
        }
    }
}
async Task PrintMembersAsync()
{
    using (var uow = new UnitOfWork())
    {
        Console.WriteLine($"Database path: {uow.GetDbPath()}");

        var list = await uow.Members.FindAllAsync();
        if (list.Count == 0)
        {
            Console.WriteLine("\n There are no Members yet");
        }
        else
        {
            Console.WriteLine("\n Members:");
            foreach (var member in list)
            {
                Console.WriteLine($"Member: {member.Id}, " +
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

async Task PrintBookingsAsync()
{
    using (var uow = new UnitOfWork())
    {
        Console.WriteLine($"Database path: {uow.GetDbPath()}");

        var list = await uow.Bookings.FindAllAsync();
        if (list.Count == 0)
        {
            Console.WriteLine("\n There are no bookings yet");
        }
        else
        {
            Console.WriteLine("\n Bookings:");
            foreach (var booking in list)
            {
                Console.WriteLine($"Booking: {booking.BookingDate}, " +
                    $"Class ID: {booking.ClassId}, " +
                    $"Member ID: {booking.MemberId}");
                
            }
        }
    }

}

async Task AddClassAsync()
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

async Task AddMemberShipAsync()
{
    using (var uow = new UnitOfWork())
    {
        Membership mb1 = new("Test", 150, "Test Description", 15, 50);
        await uow.Memberships.AddMembershipAsync(mb1);
        await uow.SaveChangesAsync();
    }
}

async Task AddTrainerAsync()
{
    using (var uow = new UnitOfWork())
    {
        Trainer t1 = new("Test Name", "password", "test@gmail.com", "testuser", "+123456789");
        await uow.Trainers.AddTrainerAsync(t1);
        await uow.SaveChangesAsync();
    }
}

async Task AddMemberAsync()
{
    using (var uow = new UnitOfWork())
    {
        Membership mb1 = new("Test", 150, "Test Description", 15, 50);
        Member m1 = new("TestName", "test@gmail.com", "password", "testuser1", "+123456789", 82.1F, 1.91F, 15, mb1);
        await uow.Members.AddMemberAsync(m1);
        await uow.SaveChangesAsync();
    }
}
static async Task AddBookingAsync()
{
    using (var uow = new UnitOfWork())
    {
        DateTime date1 = new(2025, 11, 26);
        Trainer t1 = new("Test Name","password","test@gmail.com","testuser","+123456789");
        await uow.Trainers.AddTrainerAsync(t1);
      
        Membership mb1 = new("Test", 150, "Test Description", 15, 50);
        await uow.Memberships.AddMembershipAsync(mb1);
        
        Member m1 = new("TestName", "test@gmail.com", "password", "testuser1", "+123456789", 82.1F, 1.91F, 15, mb1);
        await uow.Members.AddMemberAsync(m1);

        Class c1 = new("TestName", "TestDescription", date1, 50, t1);
        await uow.Classes.AddClassAsync(c1);

        Booking b1 = new(m1.Id, c1.Id, DateTime.Now);
        await uow.Bookings.AddBookingAsync(b1);
        await uow.SaveChangesAsync();
    }


}

async Task PrintUpcomingClassesForAllMembers()
{
    using (var uow = new UnitOfWork())
    {
        var members = await uow.Members.FindAllAsync();
        foreach (var member in members)
        {
            var classes = await uow.Bookings.GetClassesByMemberAsync(member.Id);
            Console.WriteLine($"Member: {member.FullName}");
            foreach (var c in classes)
            {
                Console.WriteLine($"Class: {c.Name}, Date: {c.ScheduledDate}");
            }
        }
    }
}

async Task PrintClassesByTrainer()
{
    using (var uow = new UnitOfWork())
    {
        var trainers = await uow.Trainers.FindAllAsync();
        foreach (var trainer in trainers)
        {
            var classes = await uow.Classes.GetClassesByTrainerAsync(trainer.Id);
            Console.WriteLine($"Trainer: {trainer.FullName}");
            foreach (var c in classes)
            {
                Console.WriteLine($"Class: {c.Name}, Date: {c.ScheduledDate}");
            }
        }
    }
}

async Task PrintAllBookedClassesByTrainer()
{
    using (var uow = new UnitOfWork())
    {
        var trainers = await uow.Trainers.FindAllAsync();
        foreach (var trainer in trainers)
        {
            var classes = await uow.Classes.GetBookedClassesByTrainerAsync(trainer.Id);
            Console.WriteLine($"Trainer: {trainer.FullName}");
            foreach (var c in classes)
            {
                Console.WriteLine($"Class: {c.Name}, Date: {c.ScheduledDate}");
            }
        }
    }
}
