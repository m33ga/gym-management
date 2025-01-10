using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using GymManagement.UWP.ViewModels;
using Windows.UI.Xaml.Navigation;
using GymManagement.UWP.Views.Booking;
using GymManagement.UWP.Views.Users;
using Windows.UI.Xaml.Media;
using System.Diagnostics;
using GymManagement.Infrastructure;

//using GymManagement.UWP.Views.Dashboard;
//using GymManagement.UWP.Views.MealPlan;
//using GymManagement.UWP.Views.Notifications;


namespace GymManagement.UWP.Views.Profile
{
    public sealed partial class ProfilePage : Page
    {
        public ProfileViewModel ViewModel { get; }

        public ProfilePage()
        {
            this.InitializeComponent();

            // Pass UnitOfWork and UserViewModel
            ViewModel = new ProfileViewModel(App.UnitOfWork, App.UserViewModel);
            DataContext = ViewModel;

            // Subscribe to property changes
            ViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(ViewModel.IsEditing) ||
                    e.PropertyName == nameof(ViewModel.IsMember) ||
                    e.PropertyName == nameof(ViewModel.IsTrainer))
                {
                    UpdateEditingMode();
                }
            };

            DisplayDatabaseInfo();
            // Set initial visibility
            UpdateEditingMode();
        }

        private void DisplayDatabaseInfo()
        {
            try
            {
                // Get the database path from UnitOfWork
                string dbPath = App.UnitOfWork.GetDbPath();

                // Update the TextBlock
                DatabasePathTextBlock.Text = dbPath;

                // Optionally log the path
                Debug.WriteLine($"Database Path: {dbPath}");
            }
            catch (Exception ex)
            {
                DatabasePathTextBlock.Text = "Error retrieving database info.";
                Debug.WriteLine($"Error fetching database info: {ex.Message}");
            }
        }


        private void UpdateEditingMode()
        {
            // Member section fields
            FullNameTextBox.IsReadOnly = !ViewModel.IsEditing;
            UsernameTextBox.IsReadOnly = !ViewModel.IsEditing;
            EmailTextBox.IsReadOnly = !ViewModel.IsEditing;
            PhoneNumberTextBox.IsReadOnly = !ViewModel.IsEditing;

            // Trainer section fields
            TrainerFullNameTextBox.IsReadOnly = !ViewModel.IsEditing;
            TrainerUsernameTextBox.IsReadOnly = !ViewModel.IsEditing;
            TrainerEmailTextBox.IsReadOnly = !ViewModel.IsEditing;
            TrainerPhoneNumberTextBox.IsReadOnly = !ViewModel.IsEditing;

            // Handle height and weight visibility for members
            if (ViewModel.IsMember)
            {
                HeightTextBox.IsReadOnly = !ViewModel.IsEditing;
                WeightTextBox.IsReadOnly = !ViewModel.IsEditing;
                HeightTextBox.Visibility = Visibility.Visible;
                WeightTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                HeightTextBox.Visibility = Visibility.Collapsed;
                WeightTextBox.Visibility = Visibility.Collapsed;
            }

            // Change background color for editing mode
            var backgroundColor = ViewModel.IsEditing ? Windows.UI.Colors.White : Windows.UI.Colors.LightGray;

            // Member section
            FullNameTextBox.Background = new SolidColorBrush(backgroundColor);
            UsernameTextBox.Background = new SolidColorBrush(backgroundColor);
            EmailTextBox.Background = new SolidColorBrush(backgroundColor);
            PhoneNumberTextBox.Background = new SolidColorBrush(backgroundColor);

            // Trainer section
            TrainerFullNameTextBox.Background = new SolidColorBrush(backgroundColor);
            TrainerUsernameTextBox.Background = new SolidColorBrush(backgroundColor);
            TrainerEmailTextBox.Background = new SolidColorBrush(backgroundColor);
            TrainerPhoneNumberTextBox.Background = new SolidColorBrush(backgroundColor);

            // Upload button visibility in edit mode
            UploadButton.Visibility = ViewModel.IsEditing ? Visibility.Visible : Visibility.Collapsed;

            // Role-based visibility
            MemberSection.Visibility = ViewModel.IsMember ? Visibility.Visible : Visibility.Collapsed;
            TrainerSection.Visibility = ViewModel.IsTrainer ? Visibility.Visible : Visibility.Collapsed;
        }



        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (DataContext is ProfileViewModel viewModel)
            {
                await viewModel.LoadProfilePictureAsync();
            }
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
                        break;
                    case "dashboard":
                        break;
                    case "schedule":
                        NavigateToSchedule();
                        break;
                    case "meal_plan":
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
