using System;
using GymManagement.Infrastructure;
using GymManagement.Domain;
using GymManagement.Application;
using GymManagement.Domain.Models;

static async Task AddBookingAsync()
{
    using (var uow = new UnitOfWork())
    {
        Trainer t1 = new("Test Name","password","test@gmail.com","testuser","+123456789");
        await uow.Trainers.AddTrainerAsync(t1);
      
        Membership mb1 = new("Test", 150, "Test Description", 15, 50);
        await uow.Memberships.AddMembershipAsync(mb1);

        await uow.SaveChangesAsync();
    }


}

await AddBookingAsync();