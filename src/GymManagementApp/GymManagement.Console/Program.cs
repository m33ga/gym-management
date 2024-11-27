using System;
using GymManagement.Infrastructure;
using GymManagement.Domain;
using GymManagement.Application;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Gym Management System");
        // Initialize the database
        using (var dbContext = new GymManagementDbContext())
        {
            // Test
            Console.WriteLine("Database connected successfully!");
            
        }
    }
}
