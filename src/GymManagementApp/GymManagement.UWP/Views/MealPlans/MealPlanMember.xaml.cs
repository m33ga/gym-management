using GymManagement.UWP.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Printing;
using Windows.Graphics.Printing;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Navigation;

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

        private PrintDocument _printDocument;
        private IPrintDocumentSource _printDocumentSource;
        private List<string> _mealsToPrint;

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

        private void InitializePrint()
        {
            _printDocument = new PrintDocument();
            _printDocumentSource = _printDocument.DocumentSource;

            // Attach event handlers
            _printDocument.Paginate += OnPaginate;
            _printDocument.GetPreviewPage += OnGetPreviewPage;
            _printDocument.AddPages += OnAddPages;

            PrintManager printManager = PrintManager.GetForCurrentView();
            printManager.PrintTaskRequested += OnPrintTaskRequested;
        }

        private void UnregisterPrint()
        {
            if (_printDocument != null)
            {
                _printDocument.Paginate -= OnPaginate;
                _printDocument.GetPreviewPage -= OnGetPreviewPage;
                _printDocument.AddPages -= OnAddPages;
            }

            PrintManager printManager = PrintManager.GetForCurrentView();
            printManager.PrintTaskRequested -= OnPrintTaskRequested;
        }

        private void OnPrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            PrintTask printTask = args.Request.CreatePrintTask("Print Meals", sourceRequested =>
            {
                sourceRequested.SetSource(_printDocumentSource);
            });
        }

        private void OnPaginate(object sender, PaginateEventArgs e)
        {
            // Assuming a single page
            _printDocument.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }

        private void OnGetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            // Create a preview page
            Grid previewPage = CreatePrintPage();
            _printDocument.SetPreviewPage(e.PageNumber, previewPage);
        }

        private void OnAddPages(object sender, AddPagesEventArgs e)
        {
            // Add the actual page to print
            Grid printPage = CreatePrintPage();
            _printDocument.AddPage(printPage);
            _printDocument.AddPagesComplete();
        }

        private Grid CreatePrintPage()
        {
            // Create a simple Grid for printing
            Grid printPage = new Grid();
            printPage.Margin = new Thickness(20);

            // Add a title
            TextBlock title = new TextBlock
            {
                Text = $"Meals for {ViewModel.SelectedDay}",
                FontSize = 24,
                FontWeight = Windows.UI.Text.FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 20)
            };
            printPage.Children.Add(title);

            // Add meal details
            ListView mealList = new ListView
            {
                ItemsSource = _mealsToPrint, // Meals for the selected day
                Margin = new Thickness(0, 40, 0, 0)
            };
            printPage.Children.Add(mealList);

            return printPage;
        }

        private void OnPrintMealsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if there are meals to print
                if (ViewModel.SelectedDayMeals == null || !ViewModel.SelectedDayMeals.Any())
                {
                    System.Diagnostics.Debug.WriteLine("No meals available to print.");
                    return;
                }

                // Populate meals for the selected day
                _mealsToPrint = ViewModel.SelectedDayMeals
                    .Select(meal => $"{meal.Name} - {meal.TotalCalories} kcal")
                    .ToList();

                // Initialize the printing process
                InitializePrint();

                // Show the print UI
                PrintManager.ShowPrintUIAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during printing: {ex.Message}");
            }
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            UnregisterPrint();
        }


    }
}
