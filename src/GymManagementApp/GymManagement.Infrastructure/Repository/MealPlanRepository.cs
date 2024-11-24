using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class MealPlanRepository : IMealPlanRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public MealPlanRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddMealPlanAsync(MealPlan mealPlan)
        {
            await _dbContext.MealPlans.AddAsync(mealPlan);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<MealPlan>> GetMealPlansByTrainerAsync(int trainerId)
        {
            return await _dbContext.MealPlans
                .Where(mp => mp.TrainerId == trainerId)
                .ToListAsync();
        }

        public async Task<IList<MealPlan>> GetMealPlansByMemberAsync(int memberId)
        {
            return await _dbContext.MealPlans
                .Where(mp => mp.MemberId == memberId)
                .ToListAsync();
        }

        public async Task<MealPlan> GetByIdWithDetailsAsync(int id)
        {
            return await _dbContext.MealPlans
                .Include(mp => mp.Meals)
                .ThenInclude(m => m.Ingredients)
                .FirstOrDefaultAsync(mp => mp.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Create(MealPlan entity)
        {
            _dbContext.MealPlans.Add(entity);
        }

        public void Update(MealPlan entity)
        {
            _dbContext.MealPlans.Update(entity);
        }

        public void Delete(MealPlan entity)
        {
            _dbContext.MealPlans.Remove(entity);
        }

        public async Task<MealPlan> FindByIdAsync(int id)
        {
            return await _dbContext.MealPlans.FindAsync(id);
        }

        public async Task<MealPlan> FindOrCreateAsync(MealPlan entity)
        {
            var existing = await _dbContext.MealPlans.FirstOrDefaultAsync(mp => mp.Id == entity.Id);
            if (existing != null)
                return existing;

            await _dbContext.MealPlans.AddAsync(entity);
            return entity;
        }

        public async Task<List<MealPlan>> FindAllAsync()
        {
            return await _dbContext.MealPlans.ToListAsync();
        }
    }
}
