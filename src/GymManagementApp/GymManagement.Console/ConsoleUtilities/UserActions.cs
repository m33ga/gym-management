using System;
using System.Threading.Tasks;
using GymManagement.Console.ConsoleUtilities;
using GymManagement.Domain.Models;
using GymManagement.Infrastructure;


namespace GymManagement.ConsoleUtilities
{
    public static class UserActions
    {
        public static async Task RegisterAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("============ Register ============");
            System.Console.WriteLine("Select Role: [1] Admin  [2] Trainer  [3] Member");
            var roleInput = System.Console.ReadLine();

            string role = roleInput switch
            {
                "1" => "Admin",
                "2" => "Trainer",
                "3" => "Member",
                _ => null
            };

            if (role == null)
            {
                System.Console.WriteLine("Invalid role. Returning to main menu.");
                System.Console.ReadKey();
                return;
            }

            using (var uow = new UnitOfWork())
            {
                System.Console.Write("Email: ");
                var email = System.Console.ReadLine();

                bool emailExists = role switch
                {
                    "Admin" => await uow.Admins.GetAdminByEmailAsync(email) != null,
                    "Trainer" => await uow.Trainers.GetTrainerByEmailAsync(email) != null,
                    "Member" => await uow.Members.GetMemberByEmailAsync(email) != null,
                    _ => throw new InvalidOperationException("Invalid role.")
                };

                if (emailExists)
                {
                    System.Console.WriteLine($"{role} with this email already exists.");
                    System.Console.ReadKey();
                    return;
                }

                System.Console.Write("Password: ");
                var password = System.Console.ReadLine();
                var hashedPassword = PasswordUtils.HashPassword(password);

                if (role == "Admin")
                {
                    System.Console.Write("Enter Admin Username: ");
                    var username = System.Console.ReadLine();

                    var admin = new Admin(username, email, hashedPassword);
                    await uow.Admins.AddAdminAsync(admin);
                    System.Console.WriteLine("Admin registered successfully.");
                }
                else if (role == "Trainer")
                {
                    System.Console.Write("Full Name: ");
                    var fullName = System.Console.ReadLine();
                    System.Console.Write("Phone Number: ");
                    var phone = System.Console.ReadLine();

                    var trainer = new Trainer(fullName, hashedPassword, email, "TrainerUsername", phone);
                    await uow.Trainers.AddTrainerAsync(trainer);
                    System.Console.WriteLine("Trainer registered successfully.");
                }
                else if (role == "Member")
                {
                    System.Console.Write("Full Name: ");
                    var fullName = System.Console.ReadLine();
                    System.Console.Write("Phone Number: ");
                    var phone = System.Console.ReadLine();
                    System.Console.Write("Weight (kg): ");
                    var weight = float.Parse(System.Console.ReadLine());
                    System.Console.Write("Height (cm): ");
                    var height = float.Parse(System.Console.ReadLine());

                    var membership = await MembershipActions.EnsureDefaultMembershipAsync(uow);

                    var member = new Member(fullName, email, hashedPassword, "MemberUsername", phone, weight, height, 10, membership);
                    await uow.Members.AddMemberAsync(member);
                    System.Console.WriteLine("Member registered successfully.");
                }

                await uow.SaveChangesAsync();
                System.Console.WriteLine("Registration Complete!");
                System.Console.ReadKey();
            }
        }

        public static async Task<string> LoginAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("============ Login ============");
            System.Console.WriteLine("Select Role: [1] Admin  [2] Trainer  [3] Member");
            var roleInput = System.Console.ReadLine();

            string role = roleInput switch
            {
                "1" => "Admin",
                "2" => "Trainer",
                "3" => "Member",
                _ => null
            };

            if (role == null)
            {
                System.Console.WriteLine("Invalid role. Returning to main menu.");
                System.Console.ReadKey();
                return null;
            }

            System.Console.Write("Email: ");
            var email = System.Console.ReadLine();
            System.Console.Write("Password: ");
            var password = System.Console.ReadLine();

            var hashedPassword = PasswordUtils.HashPassword(password);

            using (var uow = new UnitOfWork())
            {
                if (role == "Admin")
                {
                    var admin = await uow.Admins.GetAdminByEmailAsync(email);
                    if (admin != null && admin.Password == hashedPassword)
                    {
                        System.Console.WriteLine("Login Successful!");
                        System.Console.ReadKey();
                        return "Admin";
                    }
                }
                else if (role == "Trainer")
                {
                    var trainer = await uow.Trainers.GetTrainerByEmailAsync(email);
                    if (trainer != null && trainer.Password == hashedPassword)
                    {
                        System.Console.WriteLine("Login Successful!");
                        System.Console.ReadKey();
                        return "Trainer";
                    }
                }
                else if (role == "Member")
                {
                    var member = await uow.Members.GetMemberByEmailAsync(email);
                    if (member != null && member.Password == hashedPassword)
                    {
                        System.Console.WriteLine("Login Successful!");
                        System.Console.ReadKey();
                        return "Member";
                    }
                }

                System.Console.WriteLine("Invalid email or password.");
                System.Console.ReadKey();
                return null;
            }
        }
    }
}
