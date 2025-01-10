using GymManagement.Domain.Models;
using System.Collections.Generic;

namespace GymManagement.Domain.Services
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetTomorrowNotifications(int userId);
    }
}
