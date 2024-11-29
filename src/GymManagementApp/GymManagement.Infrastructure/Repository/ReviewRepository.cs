using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public ReviewRepository(GymManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddReviewAsync(Review review)
        {
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();
        }

        public override async Task<Review> FindOrCreateAsync(Review entity)
        {
            var review = await _dbContext.Reviews.FindAsync(entity.Id);
            if (review == null)
            {
                await AddReviewAsync(entity);
                review = entity;
            }
            return review;
        }

        public async Task<Review> GetReviewByMemberAndClassAsync(int memberId, int classId)
        {
            return await _dbContext.Reviews
                .FirstOrDefaultAsync(r => r.MemberId == memberId && r.ClassId == classId);
        }

        public async Task<IList<Review>> GetReviewsByTrainerAsync(int trainerId)
        {
            return await _dbContext.Reviews
                .Where(r => r.TrainerId == trainerId)
                .ToListAsync();
        }

        public async Task<IList<Review>> GetTrainerRatingAsync(int trainerId)
        {
            return await GetReviewsByTrainerAsync(trainerId);            
        }
    }
}
