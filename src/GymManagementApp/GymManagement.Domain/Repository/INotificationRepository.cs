using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Repository
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task AddNotificationAsync(Notification notification);

        // get notifications for a member
        Task<IList<Notification>> GetNotificationsByMemberAsync(int memberId);

        // get unread notifications for a member
        Task<IList<Notification>> GetUnreadNotificationsByMemberAsync(int memberId);

        Task MarkAsReadAsync(int notificationId);

        
    }
}
