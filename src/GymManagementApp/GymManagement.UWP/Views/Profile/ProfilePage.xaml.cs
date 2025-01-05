using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using GymManagement.UWP.ViewModels;
using Windows.UI.Xaml.Navigation;
using GymManagement.UWP.Views.Booking;
using GymManagement.UWP.Views.Users;

namespace GymManagement.UWP.Views.Profile
{
    public sealed partial class ProfilePage : Page
    {
        public ProfileViewModel ViewModel { get; }

        public ProfilePage()
        {
            this.InitializeComponent();
            ViewModel = new ProfileViewModel(App.UserViewModel);
            DataContext = ViewModel;

            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            MemberSection.Visibility = ViewModel.IsMember ?
                Visibility.Visible : Visibility.Collapsed;

            TrainerSection.Visibility = ViewModel.IsTrainer ?
                Visibility.Visible : Visibility.Collapsed;
        }

        private void NvMain_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem selectedItem)
            {
                switch (selectedItem.Tag)
                {
                    case "profile":
                        frmMain.Navigate(typeof(ProfilePage));
                        break;
                    case "notifications":
                        frmMain.Navigate(typeof(NotificationsPage));
                        break;
                    case "dashboard":
                        frmMain.Navigate(typeof(DashboardPage));
                        break;
                    case "schedule":
                        NavigateToSchedule();
                        break;
                    case "meal_plan":
                        frmMain.Navigate(typeof(MealPlanPage));
                        break;
                }
            }
        }

        private void NavigateToSchedule()
        {
            if (ViewModel.IsMember)
            {
                frmMain.Navigate(typeof(BookingManagementPage));
            }
            else if (ViewModel.IsTrainer || ViewModel.IsAdmin)
            {
                frmMain.Navigate(typeof(ScheduleManagementPage));
            }
        }

        private void NvLogout_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            App.UserViewModel.DoLogout();
            Frame.Navigate(typeof(LoginDialog));
        }
    }
}
