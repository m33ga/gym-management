using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public AdminRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // IAdminRepository: specific methods
        public async Task<IList<Admin>> GetAllAdminsAsync()
        {
            return await _dbContext.Admins.ToListAsync();
        }

        public async Task AddAdminAsync(Admin admin)
        {
            await _dbContext.Admins.AddAsync(admin);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAdminAsync(Admin admin)
        {
            _dbContext.Admins.Remove(admin);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        // Base IRepository methods
        public void Create(Admin entity)
        {
            _dbContext.Admins.Add(entity);
        }

        public void Update(Admin entity)
        {
            _dbContext.Admins.Update(entity);
        }

        public void Delete(Admin entity)
        {
            _dbContext.Admins.Remove(entity);
        }

        public async Task<Admin> FindByIdAsync(int id)
        {
            return await _dbContext.Admins.FindAsync(id);
        }

        public async Task<Admin> FindOrCreateAsync(Admin entity)
        {
            var existing = await _dbContext.Admins.FirstOrDefaultAsync(a => a.Id == entity.Id);
            if (existing != null)
                return existing;

            await _dbContext.Admins.AddAsync(entity);
            return entity;
        }

        public async Task<List<Admin>> FindAllAsync()
        {
            return await _dbContext.Admins.ToListAsync();
        }
    }
}
