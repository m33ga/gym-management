using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using GymManagement.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.UWP.ViewModels
{
    public class DashboardViewModel : BindableBase
    {
        private readonly IClassRepository _classRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly UserViewModel _userViewModel;

        public DashboardViewModel(IClassRepository classRepository, IBookingRepository bookingRepository, UserViewModel userViewModel)
        {
            _classRepository = classRepository;
            _bookingRepository = bookingRepository;
            _userViewModel = userViewModel;
            LoadWorkoutsAsync();
        }

        private ObservableCollection<Class> _upcomingWorkouts;
        public ObservableCollection<Class> UpcomingWorkouts
        {
            get => _upcomingWorkouts;
            set => Set(ref _upcomingWorkouts, value);
        }

        private ObservableCollection<Class> _pastWorkouts;
        public ObservableCollection<Class> PastWorkouts
        {
            get => _pastWorkouts;
            set => Set(ref _pastWorkouts, value);
        }

        private Class _selectedWorkout;
        public Class SelectedWorkout
        {
            get => _selectedWorkout;
            set
            {
                _selectedWorkout = value;
                OnPropertyChanged(nameof(SelectedWorkout));
            }
        }

        public async Task LoadWorkoutDetailsAsync(Class selectedWorkout)
        {
            SelectedWorkout = await _classRepository.GetByIdWithDetailsAsync(selectedWorkout.Id);
        }

        private async Task LoadWorkoutsAsync()
        {
            if (_userViewModel.IsMember)
            {
                var memberId = ((Member)_userViewModel.LoggedUser).Id;
                UpcomingWorkouts = new ObservableCollection<Class>(await _bookingRepository.GetUpcomingClassesByMemberAsync(memberId));
                PastWorkouts = new ObservableCollection<Class>(await _bookingRepository.GetPastClassesByMemberAsync(memberId));
            }
            else if (_userViewModel.IsTrainer)
            {
                var trainerId = ((Trainer)_userViewModel.LoggedUser).Id;
                UpcomingWorkouts = new ObservableCollection<Class>(await _classRepository.GetUpcomingBookedClassesByTrainerAsync(trainerId));
                PastWorkouts = new ObservableCollection<Class>(await _classRepository.GetPastBookedClassesByTrainerAsync(trainerId));
            }
        }

        public string FormattedTimeRange
        {
            get
            {
                if (SelectedWorkout != null)
                {
                    var startTime = SelectedWorkout.ScheduledDate.TimeOfDay;
                    var endTime = SelectedWorkout.ScheduledDate.AddMinutes(SelectedWorkout.DurationInMinutes).TimeOfDay;
                    return $"{startTime:hh\\:mm} - {endTime:hh\\:mm}";
                }
                return string.Empty;
            }
        }
    }
}
