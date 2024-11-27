using System;
using GymManagement.Infrastructure;
using GymManagement.Domain;
using GymManagement.Application;
using GymManagement.Domain.Models;

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

        Booking b1 = new(1, 1, date1);
        await uow.Bookings.AddBookingAsync(b1);
        await uow.SaveChangesAsync();
    }


}

await AddBookingAsync();