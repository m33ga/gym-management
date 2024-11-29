using System;
using System.Threading.Tasks;
using GymManagement.Domain;
using GymManagement.Domain.Repository;
using GymManagement.Infrastructure.Repository;

namespace GymManagement.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymManagementDbContext _dbContext;

        public UnitOfWork()
        {
            _dbContext = new GymManagementDbContext();

            // Ensure database is created
            _dbContext.Database.EnsureCreated();
            _dbContext.SeedData();

            // Optionally apply migrations
            //_dbContext.Database.Migrate();
        }

        // Repository Implementations
        public IAdminRepository Admins => new AdminRepository(_dbContext);
        public IBookingRepository Bookings => new BookingRepository(_dbContext);
        public IClassRepository Classes => new ClassRepository(_dbContext);
        public IMealPlanRepository MealPlans => new MealPlanRepository(_dbContext);
        public IMemberRepository Members => new MemberRepository(_dbContext);
        public IMembershipRepository Memberships => new MembershipRepository(_dbContext);
        public INotificationRepository Notifications => new NotificationRepository(_dbContext);
        public IReviewRepository Reviews => new ReviewRepository(_dbContext);
        public ITrainerRepository Trainers => new TrainerRepository(_dbContext);

        // Dispose method to clean up resources
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        // Save changes to the database
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        // Method to expose the database path for debugging or logging purposes
        public string GetDbPath()
        {
            return _dbContext.DbPath;
        }
    }
}

