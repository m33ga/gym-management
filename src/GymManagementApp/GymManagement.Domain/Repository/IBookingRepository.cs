using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task AddBookingAsync(Booking booking);

        // check if a class is already booked
        Task<bool> IsClassBookedAsync(int classId);

        // retrieve booking by ID, including navigation properties
        Task<Booking> GetByIdWithDetailsAsync(int id);

        // get all bookings for a specific member
        Task<IList<Booking>> GetBookingsByMemberAsync(int memberId);

        Task SaveChangesAsync();
    }
}
