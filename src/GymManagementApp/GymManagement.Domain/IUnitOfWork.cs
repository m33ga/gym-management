using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Domain
{
    /// <summary>
    /// deal with database operations -> IDisposable
    /// bridge between repositories and rest of code
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IAdminRepository Admins { get; }
        IBookingRepository Bookings { get; }
        IClassRepository Classes { get; }
        IMealPlanRepository MealPlans { get; }
        IMemberRepository Members { get; }
        IMembershipRepository Memberships { get; }
        INotificationRepository Notifications { get; }
        IReviewRepository Reviews { get; }
        ITrainerRepository Trainers { get; }
        //public DbContext Context { get; }


        Task SaveChangesAsync();

        // Optional: Expose database path
        string GetDbPath(); 
        void AttachAsModified<TEntity>(TEntity entity) where TEntity : class;

    }
}
