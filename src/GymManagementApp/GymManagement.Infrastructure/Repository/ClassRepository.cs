using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class ClassRepository : IClassRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public ClassRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddClassAsync(Class trainingClass)
        {
            await _dbContext.Classes.AddAsync(trainingClass);
            await _dbContext.SaveChangesAsync();
        }

        public void Create(Class entity)
        {
            _dbContext.Classes.Add(entity);
        }

        public void Delete(Class entity)
        {
            _dbContext.Classes.Remove(entity);
        }

        public async Task<List<Class>> FindAllAsync()
        {
            return await _dbContext.Classes.ToListAsync();
        }

        public async Task<Class> FindByIdAsync(int id)
        {
            return await _dbContext.Classes.FindAsync(id);
        }

        public async Task<Class> FindOrCreateAsync(Class entity)
        {
            var existing = await _dbContext.Classes.FirstOrDefaultAsync(c => c.Id == entity.Id);
            if (existing != null)
                return existing;

            await _dbContext.Classes.AddAsync(entity);
            return entity;
        }

        public async Task<IList<Class>> GetAvailableClassesAsync()
        {
            return await _dbContext.Classes
                .Where(c => c.IsAvailable)
                .ToListAsync();
        }

        public async Task<Class> GetByIdWithDetailsAsync(int id)
        {
            return await _dbContext.Classes
                .Include(c => c.Trainer)
                .Include(c => c.Bookings)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IList<Class>> GetClassesByTrainerAsync(int trainerId)
        {
            return await _dbContext.Classes
                .Where(c => c.TrainerId == trainerId)
                .ToListAsync();
        }

        public async Task<bool> HasScheduleConflictAsync(int trainerId, DateTime startTime, DateTime endTime)
        {
            return await _dbContext.Classes.AnyAsync(c =>
                c.TrainerId == trainerId &&
                c.ScheduledDate < endTime &&
                startTime < c.ScheduledDate.AddMinutes(c.DurationInMinutes));
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(Class entity)
        {
            _dbContext.Classes.Update(entity);
        }

        public Task AddAsync(Class entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
