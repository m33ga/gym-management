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
        private Timer _timer; 
        private bool _isLoading;

        public NotificationsViewModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Notifications = new ObservableCollection<Notification>();

           
            MarkAsReadCommand = new RelayCommand(
                execute: async () => await MarkAsReadAsync(),
                canExecute: () => SelectedNotification != null && SelectedNotification.Status == "Unread"
            );

            
            _timer = new Timer(async _ => await LoadNotificationsAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }

        
        public ObservableCollection<Notification> Notifications { get; }

     
        public Notification SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                Set(ref _selectedNotification, value);
                (MarkAsReadCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        
        public ICommand MarkAsReadCommand { get; }

        public async Task LoadNotificationsAsync()
        {
            if (_isLoading) return; 
            _isLoading = true;

            try
            {
                var userViewModel = App.UserViewModel;
                if (userViewModel.LoggedUser is Member member)
                {
                   
                    var newNotifications = await _unitOfWork.Notifications.GetUnreadNotificationsByMemberAsync(member.Id);

                    foreach (var notification in newNotifications)
                    {
                        if (!Notifications.Contains(notification)) 
                        {
                            Notifications.Add(notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine($"Error loading notifications: {ex.Message}");
            }
            finally
            {
                _isLoading = false; 
            }
        }

        private async Task MarkAsReadAsync()
        {
            if (SelectedNotification == null) return;

            try
            {
                
                await _unitOfWork.Notifications.MarkAsReadAsync(SelectedNotification.Id);

                SelectedNotification.MarkAsRead();

           
                Notifications[Notifications.IndexOf(SelectedNotification)] = SelectedNotification;

                
                (MarkAsReadCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine($"Error marking notification as read: {ex.Message}");
            }
        }

       
        public void StopTimer()
        {
            _timer?.Dispose();
        }
    }
}
