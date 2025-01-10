
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
        private string _scheduledTime;
        private int durationInMinutes;
        private string _className;
        private string _classDescription;

        public ScheduleViewModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            ScheduledClasses = new ObservableCollection<Class>();
            CreateClassCommand = new RelayCommand(
                execute: async () => await CreateClassAsync(),
                canExecute: CanCreateClass
            );

            Task task = LoadScheduledClassesAsync();
        }

        public ObservableCollection<Class> ScheduledClasses { get; }

        public DateTimeOffset? ScheduledDate
        {
            get => _scheduledDate;
            set
            {
                Set(ref _scheduledDate, value);
                (CreateClassCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string ScheduledTime
        {
            get => _scheduledTime;
            set
            {
                Set(ref _scheduledTime, value);
                (CreateClassCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public int DurationInMinutes
        {
            get => durationInMinutes;
            set
            {
                Set(ref durationInMinutes, value);
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
        public ObservableCollection<string> AvailableTimeSlots { get; }

        public async Task LoadAvailableTimeSlotsAsync()
        {
            if (ScheduledDate == null) return;

            try
            {
                // Clear the current time slots
                AvailableTimeSlots.Clear();

                // Load all classes scheduled on the selected date
                var classesOnSelectedDate = (await _unitOfWork.Classes.GetClassesByDateAsync(ScheduledDate.Value.DateTime))
    .Cast<Class>();

                // Generate time slots for the entire day (e.g., 12:00 PM to 8:00 PM)
                var allTimeSlots = Enumerable.Range(12, 8)
                                             .Select(hour => new TimeSpan(hour, 0, 0).ToString(@"hh\:mm"))
                                             .ToList();

                // Filter out time slots already taken
                
                var takenTimeSlots = classesOnSelectedDate
                    .Select(cls => cls.ScheduledDate.ToString("HH:mm"))
                    .ToList();

                foreach (var slot in allTimeSlots)
                {
                    if (!takenTimeSlots.Contains(slot))
                        AvailableTimeSlots.Add(slot);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading available time slots: {ex.Message}");
            }
        }
        private bool CanCreateClass()
        {
            return ScheduledDate != null && !string.IsNullOrEmpty(ScheduledTime) && !string.IsNullOrEmpty(ClassName);
        }

        public async Task CreateClassAsync()
        {
            try
            {
                if (ScheduledDate == null || !TimeSpan.TryParse(ScheduledTime, out var time))
                {
                    await ShowDialogAsync("Error", "Please provide a valid date and time.");
                    return;
                }
                var userviewmodel = App.UserViewModel;
                var scheduledDateTime = ScheduledDate.Value.Date + time;
                Trainer trainer = (Trainer)userviewmodel.LoggedUser;

                var newClass = new Class(ClassName, ClassDescription, scheduledDateTime, durationInMinutes, trainer);
                await _unitOfWork.Classes.AddClassAsync(newClass);
                ScheduledClasses.Add(newClass);

                await ShowDialogAsync("Success", "Class successfully scheduled!");

                // Clear inputs after successful creation
                ClassName = string.Empty;
                ClassDescription = string.Empty;
                ScheduledDate = null;
                ScheduledTime = string.Empty;
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

