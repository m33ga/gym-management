using System;
using System.Text;
using GymManagement.Domain;
using GymManagement.Domain.Models;
using GymManagement.Application;
using System.Security.Cryptography;
using GymManagement.Infrastructure;

Console.WriteLine("Testing GymManagement");

var exit = false;

do
{
    Console.Clear();
    Console.WriteLine("App menu");
    Console.WriteLine("--------");

    Console.WriteLine("1. Register");
    Console.WriteLine("2. Login");
    Console.WriteLine("0. Exit");

    var option = Console.ReadLine();

    switch (option)
    {
        case "1":
            await RegisterAsync();
            break;

        case "2":
            var role = await LoginAsync();
            if (role != null)
            {
                // Navigate to role-specific post-login menu
                PostLoginMenu(role);
            }
            break;

        case "0":
            exit = true;
            break;

        default:
            Console.WriteLine("Please enter a valid choice");
            Console.ReadKey();
            break;
    }
} while (!exit);

async Task RegisterAsync()
{
    Console.Clear();
    Console.WriteLine("============ Register ============");
    Console.WriteLine("Select Role: [1] Admin  [2] Trainer  [3] Member");
    var roleInput = Console.ReadLine();

    string role = roleInput switch
    {
        "1" => "Admin",
        "2" => "Trainer",
        "3" => "Member",
        _ => null
    };

    if (role == null)
    {
        Console.WriteLine("Invalid role. Returning to main menu.");
        Console.ReadKey();
        return;
    }

    using (var uow = new UnitOfWork())
    {
        Console.Write("Email: ");
        var email = Console.ReadLine();

        // Check if the email already exists for the selected role
        bool emailExists = role switch
        {
            "Admin" => await uow.Admins.GetAdminByEmailAsync(email) != null,
            "Trainer" => await uow.Trainers.GetTrainerByEmailAsync(email) != null,
            "Member" => await uow.Members.GetMemberByEmailAsync(email) != null,
            _ => throw new InvalidOperationException("Invalid role.")
        };

        if (emailExists)
        {
            Console.WriteLine($"{role} with this email already exists.");
            Console.ReadKey();
            return;
        }

        Console.Write("Password: ");
        var password = Console.ReadLine();
        var hashedPassword = HashPassword(password);

        if (role == "Admin")
        {
            // Admin-specific input
            Console.Write("Enter Admin Username: ");
            var username = Console.ReadLine();

            // Create and add the new admin
            var admin = new Admin(username, email, hashedPassword);
            await uow.Admins.AddAdminAsync(admin);
            Console.WriteLine("Admin registered successfully.");
        }
        else if (role == "Trainer")
        {
            // Trainer-specific input
            Console.Write("Full Name: ");
            var fullName = Console.ReadLine();
            Console.Write("Phone Number: ");
            var phone = Console.ReadLine();

            // Create and add the new trainer
            var trainer = new Trainer(fullName, hashedPassword, email, "TrainerUsername", phone);
            await uow.Trainers.AddTrainerAsync(trainer);
            Console.WriteLine("Trainer registered successfully.");
        }
        else if (role == "Member")
        {
            // Member-specific input
            Console.Write("Full Name: ");
            var fullName = Console.ReadLine();
            Console.Write("Phone Number: ");
            var phone = Console.ReadLine();
            Console.Write("Weight (kg): ");
            var weight = float.Parse(Console.ReadLine());
            Console.Write("Height (cm): ");
            var height = float.Parse(Console.ReadLine());

            // Create and add the new member
            var member = new Member(fullName, email, hashedPassword, "MemberUsername", phone, weight, height, 10, null);
            await uow.Members.AddMemberAsync(member);
            Console.WriteLine("Member registered successfully.");
        }

        // Save changes for all roles
        await uow.SaveChangesAsync();
        Console.WriteLine("Registration Complete!");
        Console.ReadKey();
    }
}

async Task<string> LoginAsync()
{
    Console.Clear();
    Console.WriteLine("============ Login ============");
    Console.WriteLine("Select Role: [1] Admin  [2] Trainer  [3] Member");
    var roleInput = Console.ReadLine();

    string role = roleInput switch
    {
        "1" => "Admin",
        "2" => "Trainer",
        "3" => "Member",
        _ => null
    };

    if (role == null)
    {
        Console.WriteLine("Invalid role. Returning to main menu.");
        Console.ReadKey();
        return null;
    }

    Console.Write("Email: ");
    var email = Console.ReadLine();
    Console.Write("Password: ");
    var password = Console.ReadLine();

    var hashedPassword = HashPassword(password);

    using (var uow = new UnitOfWork())
    {
        if (role == "Admin")
        {
            var admin = await uow.Admins.GetAdminByEmailAsync(email);
            if (admin != null && admin.Password == hashedPassword)
            {
                Console.WriteLine("Login Successful!");
                Console.ReadKey();
                return "Admin";
            }
        }
        else if (role == "Trainer")
        {
            var trainer = await uow.Trainers.GetTrainerByEmailAsync(email);
            if (trainer != null && trainer.Password == hashedPassword)
            {
                Console.WriteLine("Login Successful!");
                Console.ReadKey();
                return "Trainer";
            }
        }
        else if (role == "Member")
        {
            var member = await uow.Members.GetMemberByEmailAsync(email);
            if (member != null && member.Password == hashedPassword)
            {
                Console.WriteLine("Login Successful!");
                Console.ReadKey();
                return "Member";
            }
        }

        Console.WriteLine("Invalid email or password.");
        Console.ReadKey();
        return null;
    }
}


void PostLoginMenu(string role)
{
    var exit = false;

    do
    {
        Console.Clear();
        Console.WriteLine($"====== {role} Menu ======");

        switch (role)
        {
            case "Admin":
                Console.WriteLine("1. Create Membership");
                Console.WriteLine("2. Create Member");
                Console.WriteLine("3. Create Trainer");
                Console.WriteLine("4. Print Bookings");
                Console.WriteLine("5. Print Members");
                Console.WriteLine("6. Print Trainers");
                Console.WriteLine("7. Print Memberships");
                Console.WriteLine("8. Delete Member");
                Console.WriteLine("0. Logout");
                break;

            case "Trainer":
                Console.WriteLine("1. Create Class");
                Console.WriteLine("2. Print My Classes (not yet available)");
                Console.WriteLine("0. Logout");
                break;

            case "Member":
                Console.WriteLine("1. Create Booking");
                Console.WriteLine("2. Create Membership");
                Console.WriteLine("3. Print Bookings");
                Console.WriteLine("4. Delete Member");
                Console.WriteLine("5. Update Member (not yet available)");
                Console.WriteLine("0. Logout");
                break;
        }

        var input = Console.ReadLine();

        switch (role)
        {
            case "Admin":
                AdminActions(input);
                break;

            case "Trainer":
                TrainerActions(input);
                break;

            case "Member":
                MemberActions(input);
                break;
        }

        if (input == "0")
        {
            exit = true; // Logout
        }

        Console.WriteLine("Press any key to continue");
        Console.ReadKey();

    } while (!exit);
}

async Task AdminActions(string input)
{
    switch (input)
    {
        case "1":
            await AddMemberShipAsync();
            break;
        case "2":
            await AddMemberAsync();
            break;
        case "3":
            await AddTrainerAsync();
            break;
        case "4":
            await PrintBookingsAsync();
            break;
        case "5":
            await PrintMembersAsync();
            break;
        case "6":
            await PrintTrainersAsync();
            break;
        case "7":
            await PrintMemberShipsAsync();
            break;
        case "8":
            await DeleteMemberAsync();
            break;
        default:
            Console.WriteLine("Invalid Option.");
            break;
    }
}

async Task TrainerActions(string input)
{
    switch (input)
    {
        case "1":
            await AddClassAsync();
            break;
        //case "2":
        //    await PrintTrainerClassesAsync();
        //    break;
        default:
            Console.WriteLine("Invalid Option.");
            break;
    }
}

async Task MemberActions(string input)
{
    switch (input)
    {
        case "1":
            await AddBookingAsync();
            break;
        case "2":
            await AddMemberShipAsync();
            break;
        case "3":
            await PrintBookingsAsync();
            break;
        case "4":
            await DeleteMemberAsync();
            break;
        //case "5":
        //    await UpdateMemberAsync();
        //    break;
        default:
            Console.WriteLine("Invalid Option.");
            break;
    }
}


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
        Trainer t1 = new("Test Name", "password", "test@gmail.com", "testuser", "+123456789");
        await uow.Trainers.AddTrainerAsync(t1);

        Membership mb1 = new("Test", 150, "Test Description", 15, 50);
        await uow.Memberships.AddMembershipAsync(mb1);

        Member m1 = new("TestName", "test@gmail.com", "password", "testuser1", "+123456789", 82.1F, 1.91F, 15, mb1);
        await uow.Members.AddMemberAsync(m1);

        Class c1 = new("TestName", "TestDescription", date1, 50, t1);
        await uow.Classes.AddClassAsync(c1);

        Booking b1 = new(1, 1, date1);
        await uow.Bookings.AddBookingAsync(b1);
        await uow.SaveChangesAsync();
    }


}

async Task DeleteMemberAsync()
{
    using (var uow = new UnitOfWork())
    {
        Console.WriteLine("Enter Member ID to delete:");
        if (int.TryParse(Console.ReadLine(), out int memberId))
        {
            var member = await uow.Members.FindByIdAsync(memberId);
            if (member == null)
            {
                Console.WriteLine("Member not found.");
                return;
            }

            uow.Members.Delete(member);
            await uow.SaveChangesAsync();
            Console.WriteLine("Member deleted successfully.");
        }
        else
        {
            Console.WriteLine("Invalid ID entered.");
        }
    }
}

string HashPassword(string password)
{
    using (var sha256 = SHA256.Create())
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        return Convert.ToHexString(hashBytes);
    }
}





