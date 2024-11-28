using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public AdminRepository(GymManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // Get all admins
        async Task<IList<Admin>> IAdminRepository.GetAllAdminsAsync()
        {
            return await _dbContext.Admins.ToListAsync();
        }

        // Add a new admin and save changes
        async Task IAdminRepository.AddAdminAsync(Admin admin)
        {
            await _dbContext.Admins.AddAsync(admin);
            await _dbContext.SaveChangesAsync();
        }

        // Remove an admin and save changes
        async Task IAdminRepository.RemoveAdminAsync(Admin admin)
        {
            _dbContext.Admins.Remove(admin);
            await _dbContext.SaveChangesAsync();
        }

        // Find an admin by ID
        async Task<Admin> IAdminRepository.FindByIdAsync(int id)
        {
            return await _dbContext.Admins
                .Include(a => a.Notifications) // Include notifications for the admin
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // Find or create an admin (custom logic)
        public override async Task<Admin> FindOrCreateAsync(Admin entity)
        {
            var existing = await _dbContext.Admins
                .Include(a => a.Notifications) // Include notifications
                .FirstOrDefaultAsync(a => a.Username == entity.Username);

            if (existing != null)
                return existing;

            await _dbContext.Admins.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        // Get admin by username
        async Task<Admin> IAdminRepository.GetAdminByUsernameAsync(string username)
        {
            return await _dbContext.Admins
                .Include(a => a.Notifications) // Include notifications
                .FirstOrDefaultAsync(a => a.Username == username);
        }
    }
}
