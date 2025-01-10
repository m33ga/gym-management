using GymManagement.Domain.Models;
using GymManagement.Infrastructure;
using GymManagement.UWP.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace GymManagement.UWP.ViewModels
{
    public class ScheduleViewModel : BindableBase
    {
        private readonly UnitOfWork _unitOfWork;
        private DateTimeOffset? _scheduledDate;
        private string _selectedTimeSlot;
        private int _durationInMinutes;
        private string _className;
        private string _classDescription;

        public ScheduleViewModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            ScheduledClasses = new ObservableCollection<Class>();
            AvailableTimeSlots = new ObservableCollection<string>();
            CreateClassCommand = new RelayCommand(
                execute: async () => await CreateClassAsync(),
                canExecute: CanCreateClass
            );

            // Load initial data
            Task.Run(() => LoadScheduledClassesAsync());
        }

        public ObservableCollection<Class> ScheduledClasses { get; }

        public ObservableCollection<string> AvailableTimeSlots { get; }

        public DateTimeOffset? ScheduledDate
        {
            get => _scheduledDate;
            set
            {
                Set(ref _scheduledDate, value);
                (CreateClassCommand as RelayCommand)?.RaiseCanExecuteChanged();

                // Trigger loading of available time slots when the date changes
                if (_scheduledDate.HasValue)
                {
                    _ = LoadAvailableTimeSlotsAsync();
                }
            }
        }

        public string SelectedTimeSlot
        {
            get => _selectedTimeSlot;
            set
            {
                Set(ref _selectedTimeSlot, value);
                (CreateClassCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public int DurationInMinutes
        {
            get => _durationInMinutes;
            set
            {
                Set(ref _durationInMinutes, value);
                (CreateClassCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string ClassName
        {
            get => _className;
            set => Set(ref _className, value);
        }

        public string ClassDescription
        {
            get => _classDescription;
            set => Set(ref _classDescription, value);
        }

        public ICommand CreateClassCommand { get; }

        private async Task LoadScheduledClassesAsync()
        {
            try
            {
                var classes = await _unitOfWork.Classes.FindAllAsync();
                foreach (var cls in classes)
                {
                    if (!ScheduledClasses.Contains(cls))
                        ScheduledClasses.Add(cls);
                }
            }
            catch (Exception ex)
            {
                await ShowDialogAsync("Error", $"Failed to load scheduled classes: {ex.Message}");
            }
        }

        public async Task LoadAvailableTimeSlotsAsync()
        {
            if (ScheduledDate == null) return;

            try
            {
                AvailableTimeSlots.Clear();
                Debug.WriteLine($"Loading available time slots for date: {ScheduledDate}");

                // Load classes scheduled on the selected date
                var classesOnSelectedDate = (await _unitOfWork.Classes.GetClassesByDateAsync(ScheduledDate.Value.DateTime))
                    .Cast<Class>();

                Debug.WriteLine($"Found {classesOnSelectedDate.Count()} scheduled classes.");

                // Generate time slots for the entire day (e.g., 12 PM to 8 PM)
                var allTimeSlots = Enumerable.Range(12, 8) // 12 PM to 8 PM
                                             .Select(hour => $"{hour:D2}:00") // Ensure consistent format
                                             .ToList();

                Debug.WriteLine($"Generated all time slots: {string.Join(", ", allTimeSlots)}");

                // Get time slots already taken
                var takenTimeSlots = classesOnSelectedDate
                    .Select(cls =>
                    {
                        try
                        {
                            return cls.ScheduledDate.ToString("HH:mm"); // Format each scheduled date
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error formatting ScheduledDate: {ex.Message}");
                            return string.Empty;
                        }
                    })
                    .Where(slot => !string.IsNullOrEmpty(slot))
                    .ToList();

                Debug.WriteLine($"Taken time slots: {string.Join(", ", takenTimeSlots)}");

                // Add available time slots
                foreach (var slot in allTimeSlots)
                {
                    if (!takenTimeSlots.Contains(slot))
                    {
                        AvailableTimeSlots.Add(slot);
                        Debug.WriteLine($"Added available slot: {slot}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading available time slots: {ex.Message}");
            }
        }

        private bool CanCreateClass()
        {
            return ScheduledDate != null && !string.IsNullOrEmpty(SelectedTimeSlot) && !string.IsNullOrEmpty(ClassName);
        }

        public async Task CreateClassAsync()
        {
            try
            {
                if (ScheduledDate == null || !TimeSpan.TryParse(SelectedTimeSlot, out var time))
                {
                    await ShowDialogAsync("Error", "Please provide a valid date and time.");
                    return;
                }

                var scheduledDateTime = ScheduledDate.Value.Date + time;
                Trainer trainer = await _unitOfWork.Trainers.GetTrainerByEmailAsync(App.UserViewModel.Email);
                var newClass = new Class(ClassName, ClassDescription, scheduledDateTime, DurationInMinutes, trainer);
                await _unitOfWork.Classes.AddClassAsync(newClass);
                await _unitOfWork.SaveChangesAsync();

                await ShowDialogAsync("Success", "Class successfully scheduled!");

                // Clear inputs after successful creation
                ClassName = string.Empty;
                ClassDescription = string.Empty;
                ScheduledDate = null;
                SelectedTimeSlot = string.Empty;
            }
            catch (Exception ex)
            {
                await ShowDialogAsync("Error", $"Failed to create class: {ex.Message}");
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
