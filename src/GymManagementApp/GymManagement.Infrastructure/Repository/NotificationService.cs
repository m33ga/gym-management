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
        private IBookingRepository _bookingRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMemberRepository _memberRepository;

        public NotificationService(
            IBookingRepository bookingRepository,
            INotificationRepository notificationRepository,
            IMemberRepository memberRepository)
        {
            _bookingRepository = bookingRepository;
            _notificationRepository = notificationRepository;
            _memberRepository = memberRepository;
        }
       
        public void UpdateBookingRepository(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        }

       
        public async Task GenerateNotificationsAsync()
        {
            var tomorrow = DateTime.Now.AddDays(1).Date;
            var notifications = new List<Notification>();

            
            var members = await GetAllMembersAsync();

            foreach (var member in members)
            {
                var bookings = await _bookingRepository.GetBookingsByMemberAsync(member.Id);

               
                var existingNotifications = await _notificationRepository.GetNotificationsByMemberAsync(member.Id);

                foreach (var booking in bookings.Where(b => b.BookingDate.Date == tomorrow))
                {
                   
                    if (existingNotifications.Any(n => n.Text.Contains($"{booking.BookingDate:HH:mm}")))
                    {
                        continue; 
                    }

                    var notification = new Notification(
                        $"Reminder: You have a class tomorrow at {booking.BookingDate:HH:mm}.",
                        DateTime.Now,
                        "Unread",
                        booking.Id, 
                        member.Id
                    );

                    notifications.Add(notification);
                }
            }

           
            foreach (var notification in notifications)
            {
                await _notificationRepository.AddNotificationAsync(notification);
            }
        }

      
        public async Task AddNotificationAsync(Notification notification)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            await _notificationRepository.AddNotificationAsync(notification);
        }

       
        private async Task<List<Member>> GetAllMembersAsync()
        {
            var members = await _memberRepository.GetAllMembersAsync();
            return members.ToList();
        }
    }
}
