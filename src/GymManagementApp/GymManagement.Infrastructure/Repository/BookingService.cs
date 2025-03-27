using System;
using System.Threading.Tasks;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;

namespace GymManagement.Infrastructure
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly INotificationRepository _notificationRepository;

        public BookingService(IBookingRepository bookingRepository, INotificationRepository notificationRepository)
        {
            _bookingRepository = bookingRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task AddBookingAsync(Booking booking)
        {
            // Додаємо бронювання
            await _bookingRepository.AddBookingAsync(booking);

            // Генеруємо сповіщення
            var notification = new Notification(
                $"You have successfully booked a class on {booking.BookingDate:dd MMM yyyy} at {booking.BookingDate:HH:mm}.",
                DateTime.Now,
                "Unread",
                booking.Id,
                booking.MemberId.Value
            );

            await _notificationRepository.AddNotificationAsync(notification);
        }
    }
}
