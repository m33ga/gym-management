using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;

namespace GymManagement.Infrastructure.Repository
{
    public class NotificationService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IBookingRepository bookingRepository, INotificationRepository notificationRepository)
        {
            _bookingRepository = bookingRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task GenerateNotificationsAsync()
        {
            var notifications = new List<Notification>();
            var members = GetAllMembers(); // Реалізуйте цей метод

            foreach (var member in members)
            {
                var bookings = await _bookingRepository.GetBookingsByMemberAsync(member.Id);

                // Фільтруємо заняття, які відбудуться завтра
                foreach (var booking in bookings.Where(b => b.BookingDate.Date == DateTime.Now.AddDays(1).Date))
                {
                    var notification = new Notification(
                        $"Reminder: You have a class tomorrow at {booking.BookingDate:HH:mm}.",
                        DateTime.Now,
                        "Unread",
                        adminId: null,
                        memberId: member.Id
                    );

                    notifications.Add(notification);
                }
            }

            
            foreach (var notification in notifications)
            {
                await _notificationRepository.AddNotificationAsync(notification);
            }
        }

        private List<Member> GetAllMembers()
        {
            
            throw new NotImplementedException();
        }
    }
}