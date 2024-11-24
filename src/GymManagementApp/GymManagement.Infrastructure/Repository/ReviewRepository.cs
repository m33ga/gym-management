using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public ReviewRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddReviewAsync(Review review)
        {
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();
        }

        public void Create(Review entity)
        {
            _dbContext.Reviews.Add(entity);
        }

        public void Delete(Review entity)
        {
            _dbContext.Reviews.Remove(entity);
        }

        public async Task<List<Review>> FindAllAsync()
        {
            return await _dbContext.Reviews.ToListAsync();
        }

        public async Task<Review> FindByIdAsync(int id)
        {
            return await _dbContext.Reviews.FindAsync(id);
        }

        public async Task<Review> FindOrCreateAsync(Review entity)
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

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(Review entity)
        {
            _dbContext.Reviews.Update(entity);
        }
    }
}
