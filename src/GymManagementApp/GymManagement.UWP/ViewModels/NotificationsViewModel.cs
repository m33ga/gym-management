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

            MarkAsReadCommand = new RelayCommand(
                execute: async () => await MarkAsReadAsync(),
                canExecute: () => SelectedNotification != null && SelectedNotification.Status == "Unread"
            );

            // Завантаження даних
            Task task = LoadNotificationsAsync();
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
