using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public TrainerRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task<bool> TrainerExistsByEmailAsync(string email)
        {
            return await _dbContext.Trainers.AnyAsync(t => t.Email == email);
        }

        public async Task RemoveTrainerAsync(Trainer trainer)
        {
            _dbContext.Trainers.Remove(trainer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Create(Trainer entity)
        {
            _dbContext.Trainers.Add(entity);
        }

        public void Update(Trainer entity)
        {
            _dbContext.Trainers.Update(entity);
        }

        public void Delete(Trainer entity)
        {
            _dbContext.Trainers.Remove(entity);
        }

        public async Task<List<Trainer>> FindAllAsync()
        {
            return await _dbContext.Trainers.ToListAsync();
        }

        public async Task<Trainer> FindByIdAsync(int id)
        {
            return await _dbContext.Trainers.FindAsync(id);
        }

        public async Task<Trainer> FindOrCreateAsync(Trainer entity)
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
