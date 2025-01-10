using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Repository;
using GymManagement.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public abstract class GenericRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly GymManagementDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> FindAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
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


        public async Task<T> GetByIdWithIncludeAsync(int id, params string[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public abstract Task<T> FindOrCreateAsync(T entity);

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

    }
}