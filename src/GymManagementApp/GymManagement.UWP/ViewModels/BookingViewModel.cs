using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GymManagement.Domain.Repository;
using GymManagement.Infrastructure;
using GymManagement.Domain.Models;
using Windows.UI.Xaml.Controls;
using GymManagement.UWP.Helpers;
using GymManagement.Domain.Services;
using System.Linq;
using System.Diagnostics;

namespace GymManagement.UWP.ViewModels
{
    public class BookingViewModel : BindableBase
    {
        private readonly UnitOfWork _unitOfWork;
        private DateTimeOffset? _selectedDate;
        private string _selectedTimeSlot;
        private readonly IAuthentificationService authentificationService;

        public BookingViewModel(UnitOfWork unitOfWork, IAuthentificationService authentificationService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.authentificationService = authentificationService ?? throw new ArgumentNullException(nameof(authentificationService));
            AvailableDates = new ObservableCollection<DateTime>();
            AvailableTimeSlots = new ObservableCollection<string>();

            ScheduleWorkoutCommand = new RelayCommand(
                execute: async () => await ScheduleWorkoutAsync(),
                canExecute: CanScheduleWorkout
            );

            Task task = LoadDataAsync();
        }

        public ObservableCollection<DateTime> AvailableDates { get; }
        public ObservableCollection<string> AvailableTimeSlots { get; }

        public DateTimeOffset MinDate => DateTimeOffset.Now;
        public DateTimeOffset MaxDate => DateTimeOffset.Now.AddMonths(3);

        public DateTimeOffset? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    _ = LoadAvailableTimeSlotsAsync(); // Use fire-and-forget with a proper await inside
                }
            }
        }

        public string SelectedTimeSlot
        {
            get => _selectedTimeSlot;
            set
            {
                Set(ref _selectedTimeSlot, value);
                (ScheduleWorkoutCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand ScheduleWorkoutCommand { get; }

        public async Task LoadDataAsync()
        {
            // Fetch available dates from the database
            var availableClasses = await _unitOfWork.Classes.GetAvailableClassesAsync();
            foreach (var cls in availableClasses)
            {
                if (!AvailableDates.Contains(cls.ScheduledDate))
                    AvailableDates.Add(cls.ScheduledDate);
            }
        }

        private async Task LoadAvailableTimeSlotsAsync()
        {
            if (SelectedDate == null) return;

            try
            {
                // Clear the available time slots before loading
                AvailableTimeSlots.Clear();

                // Fetch classes on the selected date
                var classesOnSelectedDate = await _unitOfWork.Classes.GetClassesByDateAsync(SelectedDate.Value.DateTime);
                
                // Ensure that classes are strongly-typed
                foreach (var cls in classesOnSelectedDate.Cast<Class>())
                {
                    // Extract only the time portion from the ScheduledDate
                    var timeString = cls.ScheduledDate.ToString("HH:mm");

                    // Add unique time slots to the collection
                    if (!AvailableTimeSlots.Contains(timeString))
                        AvailableTimeSlots.Add(timeString);
                }
            }
            catch (InvalidCastException ex)
            {
                Debug.WriteLine($"Error casting classes: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading available time slots: {ex.Message}");
            }
            Debug.WriteLine($"Available Time Slots: {string.Join(", ", AvailableTimeSlots)}");
        }

        private bool CanScheduleWorkout()
        {
            return SelectedDate != null && !string.IsNullOrEmpty(SelectedTimeSlot);
        }

        public async Task ScheduleWorkoutAsync()
        {
            try
            {
                if (SelectedDate == null || string.IsNullOrEmpty(SelectedTimeSlot))
                {
                    await ShowDialogAsync("Error", "Please select a valid date and time.");
                    return;
                }

                var dateOnly = SelectedDate.Value.Date;
                if (!TimeSpan.TryParse(SelectedTimeSlot, out var timeOnly))
                {
                    await ShowDialogAsync("Error", "Invalid time format.");
                    return;
                }

                var combinedDateTime = dateOnly + timeOnly;

                var cls = await _unitOfWork.Classes.GetClassByDateTimeAsync(combinedDateTime);
                if (cls == null)
                {
                    await ShowDialogAsync("Error", "No class found for the selected date and time.");
                    return;
                }

                var userViewModel = App.UserViewModel;
                if (!(userViewModel.LoggedUser is Member member))
                {
                    await ShowDialogAsync("Error", "Invalid member. Booking cannot proceed.");
                    return;
                }

                // Ensure foreign key validity
                var existingClass = await _unitOfWork.Classes.GetByIdWithDetailsAsync(cls.Id);
                if (existingClass == null)
                {
                    await ShowDialogAsync("Error", "The referenced class does not exist.");
                    return;
                }

                var existingMember = await _unitOfWork.Members.FindByIdAsync(member.Id);
                if (existingMember == null)
                {
                    await ShowDialogAsync("Error", "The referenced member does not exist.");
                    return;
                }

                var booking = new Booking( member.Id, cls.Id, DateTime.Now);
                Debug.WriteLine(booking.Id);
                await _unitOfWork.Bookings.AddBookingAsync(booking);
                Debug.WriteLine(booking.Id);
                Debug.WriteLine(booking.ClassId);
                Debug.WriteLine(booking.MemberId);


                await ShowDialogAsync("Success", "Workout successfully scheduled!");
            }
            catch (InvalidOperationException ex)
            {
                await ShowDialogAsync("Error", ex.Message);
            }
            catch (Exception ex)
            {
                await ShowDialogAsync("Unexpected Error", ex.Message);
            }
        }


        private async Task ShowDialogAsync(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK"
            };

            await dialog.ShowAsync();
        }
    }
}

