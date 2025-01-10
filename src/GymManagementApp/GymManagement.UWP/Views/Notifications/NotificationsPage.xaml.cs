using GymManagement.Infrastructure;
using GymManagement.UWP.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GymManagement.UWP.Views.Notifications
{
    public sealed partial class NotificationsPage : Page
    {
        public NotificationsViewModel ViewModel { get; }

        public NotificationsPage()
        {
            this.InitializeComponent();
            UnitOfWork unitOfWork = new UnitOfWork();
            ViewModel = new NotificationsViewModel(unitOfWork);
            this.DataContext = ViewModel;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadNotificationsAsync();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.StopTimer(); // Зупиняємо таймер при переході зі сторінки
            base.OnNavigatedFrom(e);
        }
    }
}
