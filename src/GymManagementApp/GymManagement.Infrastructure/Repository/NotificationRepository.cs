using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repository
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        //private readonly GymManagementDbContext _dbContext;

        public NotificationRepository(GymManagementDbContext dbContext) : base(dbContext)
        {
            //_dbContext = dbContext;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _dbContext.Notifications.AddAsync(notification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<Notification>> GetNotificationsByMemberAsync(int memberId)
        {
            return await _dbContext.Notifications
                .Where(n => n.MemberId == memberId)
                .ToListAsync();
        }

        public async Task<IList<Notification>> GetUnreadNotificationsByMemberAsync(int memberId)
        {
            return await _dbContext.Notifications
                .Where(n => n.MemberId == memberId && n.Status == "Unread")
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _dbContext.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.MarkAsRead();
                _dbContext.Notifications.Update(notification);
                await _dbContext.SaveChangesAsync();
            }
        }

        public override async Task<Notification> FindOrCreateAsync(Notification entity)
        {
            var existing = await _dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == entity.Id);
            if (existing != null)
                return existing;

            await _dbContext.Notifications.AddAsync(entity);
            return entity;
        }
    }
}
