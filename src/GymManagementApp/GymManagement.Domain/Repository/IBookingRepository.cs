﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task AddBookingAsync(Booking booking);
        // Check if a class is already booked
        Task<bool> IsClassBookedAsync(int classId);

        // Retrieve booking by ID, including navigation properties
        Task<Booking> GetByIdWithDetailsAsync(int id);

        // Get all bookings for a specific member
        Task<IList<Booking>> GetBookingsByMemberAsync(int memberId);

        Task<IList<Class>> GetClassesByMemberAsync(int memberId);

        Task<IList<Class>> GetPastClassesByMemberAsync(int memberId);
        Task<IList<Class>> GetUpcomingClassesByMemberAsync(int memberId);
    }
}
