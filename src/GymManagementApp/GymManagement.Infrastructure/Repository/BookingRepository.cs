using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public BookingRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddBookingAsync(Booking booking)
        {
            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Booking> FindByIdAsync(int id)
        {
            return await _dbContext.Bookings.FindAsync(id);
        }

        public void Update(Booking booking)
        {
            _dbContext.Bookings.Update(booking);
        }

        public void Delete(Booking booking)
        {
            _dbContext.Bookings.Remove(booking);
        }

        public async Task<List<Booking>> FindAllAsync()
        {
            return await _dbContext.Bookings.ToListAsync();
        }

        public async Task<bool> IsClassBookedAsync(int classId)
        {
            return await _dbContext.Bookings.AnyAsync(b => b.ClassId == classId);
        }

        public async Task<Booking> GetByIdWithDetailsAsync(int id)
        {
            return await _dbContext.Bookings
                .Include(b => b.Class)  
                .Include(b => b.Member) 
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IList<Booking>> GetBookingsByMemberAsync(int memberId)
        {
            return await _dbContext.Bookings
                .Where(b => b.MemberId == memberId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Create(Booking entity)
        {
            _dbContext.Bookings.Add(entity);
        }

        public async Task<Booking> FindOrCreateAsync(Booking entity)
        {
            var existing = await _dbContext.Bookings
                .FirstOrDefaultAsync(b => b.Id == entity.Id);
            if (existing != null)
                return existing;

            await _dbContext.Bookings.AddAsync(entity);
            return entity;
        }
    }
}
