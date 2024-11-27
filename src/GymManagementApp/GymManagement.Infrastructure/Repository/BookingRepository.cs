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

        // Get all bookings for a specific member
        public async Task<IList<Booking>> GetBookingsByMemberAsync(int memberId)
        {
            return await _dbContext.Bookings
                .Where(b => b.MemberId == memberId)
                .ToListAsync();
        }
    }
}
