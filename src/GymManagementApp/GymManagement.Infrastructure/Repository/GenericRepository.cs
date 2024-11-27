using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly GymManagementDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            
            return await _dbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        
        public async Task<T> GetByIdWithIncludeAsync(int id, params string[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }
    }
}