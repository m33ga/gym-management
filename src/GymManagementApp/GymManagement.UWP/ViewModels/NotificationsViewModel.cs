using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using GymManagement.Infrastructure;
using GymManagement.UWP.Helpers;

namespace GymManagement.UWP.ViewModels
{
    public class NotificationsViewModel : BindableBase
    {
        private readonly UnitOfWork _unitOfWork;
        private Notification _selectedNotification;

        public NotificationsViewModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Notifications = new ObservableCollection<Notification>();

            // Команда для позначення як прочитаного
            MarkAsReadCommand = new RelayCommand(
                execute: async () => await MarkAsReadAsync(),
                canExecute: () => SelectedNotification != null && SelectedNotification.Status == "Unread"
            );

            // Додавання тестових даних
            AddTestNotifications();
        }

        // Додавання тестових даних
        private void AddTestNotifications()
        {
            Notifications.Add(new Notification(
                text: "test 1",
                date: DateTime.Now,
                status: "Unread",
                adminId: 1,
                memberId: 2
            ));

            Notifications.Add(new Notification(
                text: "test 2",
                date: DateTime.Now.AddMinutes(-10),
                status: "Unread",
                adminId: 1,
                memberId: 2
            ));

            Notifications.Add(new Notification(
                text: "test 3 (Read)",
                date: DateTime.Now.AddHours(-1),
                status: "Read",
                adminId: 1,
                memberId: 2
            ));
        }



        // Колекція для зберігання сповіщень
        public ObservableCollection<Notification> Notifications { get; }

        // Вибране сповіщення
        public Notification SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                Set(ref _selectedNotification, value);
                (MarkAsReadCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        // Команда для позначення сповіщення як прочитаного
        public ICommand MarkAsReadCommand { get; }

        // Метод для завантаження сповіщень
        private async Task LoadNotificationsAsync()
        {
            var userViewModel = App.UserViewModel;
            if (userViewModel.LoggedUser is Member member)
            {
                // Отримання непрочитаних сповіщень для користувача
                var notifications = await _unitOfWork.Notifications.GetUnreadNotificationsByMemberAsync(member.Id);

                Notifications.Clear();
                foreach (var notification in notifications)
                {
                    Notifications.Add(notification);
                }
            }
        }

        // Метод для позначення сповіщення як прочитаного
        private async Task MarkAsReadAsync()
        {
            if (SelectedNotification == null) return;

            // Оновлюємо статус у базі даних
            await _unitOfWork.Notifications.MarkAsReadAsync(SelectedNotification.Id);

            // Викликаємо метод моделі для зміни статусу
            SelectedNotification.MarkAsRead();

            // Оновлюємо стан команди
            (MarkAsReadCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}
