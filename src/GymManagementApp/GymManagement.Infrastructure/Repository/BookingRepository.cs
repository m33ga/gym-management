using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public BookingRepository(GymManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddBookingAsync(Booking booking)
        {
            // Find the class to book
            var classToBook = await _dbContext.Classes.FindAsync(booking.ClassId);
            if (classToBook == null)
                throw new InvalidOperationException("Class not found.");

            // Find the member for the booking
            var member = await _dbContext.Members.FindAsync(booking.MemberId);
            if (member == null)
                classToBook.IsAvailable = true;

            classToBook.BookClass(booking.MemberId);
            // Add the booking to the database
            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();
        }
        // Check if a class is already booked
        public async Task<bool> IsClassBookedAsync(int classId)
        {
            return await _dbContext.Bookings.AnyAsync(b => b.ClassId == classId);
        }


        // Retrieve booking by ID, including related navigation properties
        public async Task<Booking> GetByIdWithDetailsAsync(int id)
        {
            return await _dbContext.Bookings
                .Include(b => b.Class) // Include related Class
                .Include(b => b.Member) // Include related Member
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public override async Task<Booking> FindOrCreateAsync(Booking entity)
        {
            var existing = await _dbContext.Bookings.FirstOrDefaultAsync(c => c.Id == entity.Id);
            if (existing != null)
                return existing;

            await _dbContext.Bookings.AddAsync(entity);
            return entity;
        }
        // Get all bookings for a specific member
        public async Task<IList<Booking>> GetBookingsByMemberAsync(int memberId)
        {
            return await _dbContext.Bookings
                .Where(b => b.MemberId == memberId)
                .ToListAsync();
        }

        public async Task<IList<Class>> GetClassesByMemberAsync(int memberId)
        {
            return await _dbContext.Bookings
                .Where(b => b.MemberId == memberId)
                .Select(b => b.Class)
                .ToListAsync();
        }
    }
}
