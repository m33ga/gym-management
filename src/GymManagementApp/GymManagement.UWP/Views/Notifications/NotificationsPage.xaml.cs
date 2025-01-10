using GymManagement.Domain.Services;
using GymManagement.Infrastructure;
using GymManagement.Infrastructure.Repository;
using GymManagement.UWP.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation; // Add this for navigation

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

        // Use OnNavigatedTo to load notifications when the page is navigated to
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadNotificationsAsync(); // Wait for the async method to complete
            base.OnNavigatedTo(e);
        }
    }
}
