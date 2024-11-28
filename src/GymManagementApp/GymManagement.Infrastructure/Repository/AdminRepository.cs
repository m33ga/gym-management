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
        public async Task<IList<Admin>> GetAllAdminsAsync()
        {
            return await _dbContext.Admins.ToListAsync();
        }

        // Add a new admin and save changes
        public async Task AddAdminAsync(Admin admin)
        {
            await _dbContext.Admins.AddAsync(admin);
            await _dbContext.SaveChangesAsync();
        }

        // Remove an admin and save changes
        public async Task RemoveAdminAsync(Admin admin)
        {
            _dbContext.Admins.Remove(admin);
            await _dbContext.SaveChangesAsync();
        }

        // Find an admin by ID
        public async Task<Admin> FindByIdAsync(int id)
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
        public async Task<Admin> GetAdminByUsernameAsync(string username)
        {
            return await _dbContext.Admins
                .Include(a => a.Notifications) // Include notifications
                .FirstOrDefaultAsync(a => a.Username == username);
        }
    }
}
