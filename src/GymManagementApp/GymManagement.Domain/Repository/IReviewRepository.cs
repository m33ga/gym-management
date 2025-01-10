using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task AddReviewAsync(Review review);

        Task UpdateAsync(Review review);


        // get all reviews for a specific trainer (for the aggregate review score)
        Task<IList<Review>> GetReviewsByTrainerAsync(int trainerId);

        // get a specific review by member and class
        Task<Review> GetReviewByMemberAndClassAsync(int memberId, int classId);
        Task<IList<Review>> GetTrainerRatingAsync(int trainerId);
        Task SaveChangesAsync();
    }
}
