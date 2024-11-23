using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    public interface IMealPlanRepository : IRepository<MealPlan>
    {
        Task AddMealPlanAsync(MealPlan mealPlan);

        // get meal plans created by a trainer
        Task<IList<MealPlan>> GetMealPlansByTrainerAsync(int trainerId);

        // get meal plans assigned to a member
        Task<IList<MealPlan>> GetMealPlansByMemberAsync(int memberId);

        // retrieve a meal plan with full details (meals and ingredients)
        Task<MealPlan> GetByIdWithDetailsAsync(int id);

        Task SaveChangesAsync();
    }
}
