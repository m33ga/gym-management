using GymManagement.UWP.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace GymManagement.UWP.Views.MealPlans
{
    public sealed partial class MealPlanMemberPage : Page
    {
        public MealPlanViewModel ViewModel { get; }

        public MealPlanMemberPage()
        {
            this.InitializeComponent();

            // Assuming the ViewModel is injected or instantiated here
            ViewModel = new MealPlanViewModel(App.UnitOfWork, App.UserViewModel); // Adjust based on your DI setup
            this.DataContext = ViewModel;

            Initialize();
        }

        private void Initialize()
        {
            // Set default day selection to Monday (or the current day)
            string defaultDay = DateTime.Now.DayOfWeek.ToString();
            ViewModel.SelectedDay = defaultDay;

            // Optionally trigger the command for the default day
            ViewModel.SelectDayCommand.Execute(defaultDay);
        }

        private async void OnDayButtonClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button dayButton && dayButton.Content is string day)
            {
                // Update the SelectedDay in the ViewModel
                ViewModel.SelectedDay = day;

                // Optionally, log or debug
                System.Diagnostics.Debug.WriteLine($"Day selected: {day}");

                // Fetch meals for the selected day
                await ViewModel.LoadSelectedDayMealsAsync();
            }
        }
    }
}
