using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        //private readonly GymManagementDbContext _dbContext;

        public ClassRepository(GymManagementDbContext dbContext) : base(dbContext)
        {
            //_dbContext = dbContext;
        }

        public async Task AddClassAsync(Class trainingClass)
        {
            await _dbContext.Classes.AddAsync(trainingClass);
            await _dbContext.SaveChangesAsync();
        }


        public override async Task<Class> FindOrCreateAsync(Class entity)
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
                .Include(c => c.Member)
                .Include(c => c.Booking)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IList<Class>> GetClassesByTrainerAsync(int trainerId)
        {
            return await _dbContext.Classes
                .Where(c => c.TrainerId == trainerId)
                .ToListAsync();
        }

        public async Task<IList<Class>> GetUpcomingBookedClassesByTrainerAsync(int trainerId)
        {
            return await _dbContext.Classes
                .Include(c => c.Member)
                .Where(c => c.TrainerId == trainerId && !c.IsAvailable && c.ScheduledDate > DateTime.Now)
                .OrderBy(c => c.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IList<Class>> GetUpcomingClassesByTrainerAsync(int trainerId)
        {
            return await _dbContext.Classes
                .Include(c => c.Member)
                .Where(c => c.TrainerId == trainerId && c.ScheduledDate > DateTime.Now)
                .OrderBy(c => c.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IList<Class>> GetPastBookedClassesByTrainerAsync(int trainerId)
        {
            return await _dbContext.Classes
                .Include(c => c.Member)
                .Where(c => c.TrainerId == trainerId && !c.IsAvailable && c.ScheduledDate.AddMinutes(c.DurationInMinutes) < DateTime.Now)
                .OrderByDescending(c => c.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IList<Class>> GetPastBookedClassesByMemberAsync(int memberId)
        {
            return await _dbContext.Classes
                .Include(c => c.Trainer)
                .Where(c => c.MemberId == memberId && !c.IsAvailable && c.ScheduledDate.AddMinutes(c.DurationInMinutes) < DateTime.Now)
                .OrderByDescending(c => c.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IList<Class>> GetUpcomingBookedClassesByMemberAsync(int memberId)
        {
            return await _dbContext.Classes
                .Include(c => c.Trainer)
                .Where(c => c.MemberId == memberId && !c.IsAvailable && c.ScheduledDate > DateTime.Now)
                .OrderBy(c => c.ScheduledDate)
                .ToListAsync();
        }


        //public async Task<IList<Class>> GetBookedClassesByTrainerAsync(int trainerId)
        //{
        //    return await _dbContext.Classes
        //        .Where(c => c.TrainerId == trainerId && !c.IsAvailable)
        //        .ToListAsync();
        //}

        public async Task<bool> HasScheduleConflictAsync(int trainerId, DateTime startTime, DateTime endTime)
        {
            return await _dbContext.Classes.AnyAsync(c =>
                c.TrainerId == trainerId &&
                c.ScheduledDate < endTime &&
                startTime < c.ScheduledDate.AddMinutes(c.DurationInMinutes));
        }

        public async Task<IEnumerable<object>> GetClassesByDateAsync(DateTime dateTime)
        {
            return await _dbContext.Classes
               .Where(c => c.ScheduledDate.Date == dateTime.Date)
               .ToListAsync();
        }


        public async Task<Class> GetClassByDateTimeAsync(DateTime scheduledDateTime)
        {
            return await _dbContext.Classes
                .Where(c => c.ScheduledDate == scheduledDateTime)
                .FirstOrDefaultAsync();
        }


    }
}
