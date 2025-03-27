using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class MealPlanRepository : GenericRepository<MealPlan>, IMealPlanRepository
    {
        //private readonly GymManagementDbContext _dbContext;

        public MealPlanRepository(GymManagementDbContext dbContext) : base(dbContext)
        {
            //_dbContext = dbContext;
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


        public override async Task<MealPlan> FindOrCreateAsync(MealPlan entity)
        {
            var existing = await _dbContext.MealPlans.FirstOrDefaultAsync(mp => mp.Id == entity.Id);
            if (existing != null)
                return existing;

            await _dbContext.MealPlans.AddAsync(entity);
            return entity;
        }

        public async Task<MealPlan> GetMealPlanByDayAsync(string day, int userId)
        {
            return await _dbContext.MealPlans
                .Include(mp => mp.Meals)
                .FirstOrDefaultAsync(mp => mp.Meals.Any(m => m.DayOfWeek == int.Parse(day))
                                            && (mp.MemberId == userId || mp.TrainerId == userId));


        }


    }
}
