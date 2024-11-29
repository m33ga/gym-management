using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class TrainerRepository : GenericRepository<Trainer>, ITrainerRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public TrainerRepository(GymManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Trainer> GetByIdWithDetailsAsync(int id)
        {
            return await _dbContext.Trainers
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task AddTrainerAsync(Trainer trainer)
        {
            await _dbContext.Trainers.AddAsync(trainer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Trainer> GetByUsernameAsync(string username)
        {
            return await _dbContext.Trainers.FirstOrDefaultAsync(t => t.Username == username);
        }

        public async Task<IList<Trainer>> GetTrainersWithClassesAsync()
        {
            return await _dbContext.Trainers
                .Include(t => t.Classes)
                .ToListAsync();
        }

        public async Task<Trainer> GetTrainerByEmailAsync(string email)
        {
            return await _dbContext.Trainers
                .Include(t => t.Classes) // Include scheduled classes
                .FirstOrDefaultAsync(t => t.Email == email);
        }

        public async Task RemoveTrainerAsync(Trainer trainer)
        {
            _dbContext.Trainers.Remove(trainer);
            await _dbContext.SaveChangesAsync();
        }

        public override async Task<Trainer> FindOrCreateAsync(Trainer entity)
        {
            var trainer = await _dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == entity.Id);
            if (trainer == null)
            {
                await AddTrainerAsync(entity);
                trainer = entity;
            }
            return trainer;
        }
    }
}
