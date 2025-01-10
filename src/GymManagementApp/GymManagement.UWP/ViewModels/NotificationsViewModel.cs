using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GymManagement.Domain.Models;
using GymManagement.Infrastructure;
using GymManagement.UWP.Helpers;

namespace GymManagement.UWP.ViewModels
{
    public class NotificationsViewModel : BindableBase
    {
        private readonly UnitOfWork _unitOfWork;
        private Notification _selectedNotification;
        private Timer _timer; // Таймер для періодичної перевірки
        private bool _isLoading; // Запобігає накладенню запитів під час виконання

        public NotificationsViewModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Notifications = new ObservableCollection<Notification>();

            // Ініціалізація команди для позначення сповіщення як прочитаного
            MarkAsReadCommand = new RelayCommand(
                execute: async () => await MarkAsReadAsync(),
                canExecute: () => SelectedNotification != null && SelectedNotification.Status == "Unread"
            );

            // Налаштування таймера для перевірки нових сповіщень
            _timer = new Timer(async _ => await LoadNotificationsAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
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

        // Метод для завантаження сповіщень із бази даних
        public async Task LoadNotificationsAsync()
        {
            if (_isLoading) return; // Перевірка, чи метод вже виконується
            _isLoading = true;

            try
            {
                var userViewModel = App.UserViewModel;
                if (userViewModel.LoggedUser is Member member)
                {
                    // Отримуємо непрочитані сповіщення для користувача
                    var newNotifications = await _unitOfWork.Notifications.GetUnreadNotificationsByMemberAsync(member.Id);

                    foreach (var notification in newNotifications)
                    {
                        if (!Notifications.Contains(notification)) // Перевірка на дублікати
                        {
                            Notifications.Add(notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обробка помилок, якщо необхідно
                System.Diagnostics.Debug.WriteLine($"Error loading notifications: {ex.Message}");
            }
            finally
            {
                _isLoading = false; // Завершення виконання
            }
        }

        // Метод для позначення сповіщення як прочитаного
        private async Task MarkAsReadAsync()
        {
            if (SelectedNotification == null) return;

            try
            {
                // Оновлення статусу у базі даних
                await _unitOfWork.Notifications.MarkAsReadAsync(SelectedNotification.Id);

                // Виклик методу моделі для зміни статусу
                SelectedNotification.MarkAsRead();

                // Оновлення стану колекції
                Notifications[Notifications.IndexOf(SelectedNotification)] = SelectedNotification;

                // Оновлення стану команди
                (MarkAsReadCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                // Обробка помилок, якщо необхідно
                System.Diagnostics.Debug.WriteLine($"Error marking notification as read: {ex.Message}");
            }
        }

        // Метод для зупинки таймера при закритті
        public void StopTimer()
        {
            _timer?.Dispose();
        }
    }
}
