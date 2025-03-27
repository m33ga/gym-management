using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GymManagement.Domain.Enums;
using GymManagement.Domain;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using GymManagement.UWP.Helpers;
using System.Diagnostics;
using System;
using System.Linq;

namespace GymManagement.UWP.ViewModels
{
    public class MealPlanViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserViewModel _userViewModel;

        public string DefaultBreakfastImage => "ms-appx:///Assets/DefaultBreakfast.png";
        public string DefaultLunchImage => "ms-appx:///Assets/DefaultLunch.png";
        public string DefaultDinnerImage => "ms-appx:///Assets/DefaultDinner.png";

        // Role identification
        public bool IsMember => _userViewModel.LoggedUser is Member;
        public bool IsTrainer => _userViewModel.LoggedUser is Trainer;

        private Member _selectedMember;
        public Member SelectedMember
        {
            get => _selectedMember;
            set => Set(ref _selectedMember, value);
        }

        // Data for Members and Trainers
        private ObservableCollection<MealPlan> _mealPlans;
        public ObservableCollection<MealPlan> MealPlans
        {
            get => _mealPlans;
            set => Set(ref _mealPlans, value);
        }

        private ObservableCollection<Meal> _selectedDayMeals;
        public ObservableCollection<Meal> SelectedDayMeals
        {
            get => _selectedDayMeals;
            set => Set(ref _selectedDayMeals, value);
        }

        private string _selectedDay;
        public string SelectedDay
        {
            get => _selectedDay;
            set
            {
                if (Set(ref _selectedDay, value))
                {
                    // Load meals when the selected day changes
                    LoadSelectedDayMealsAsync();
                }
            }
        }

        private string _selectedDayTotalCalories;
        public string SelectedDayTotalCalories
        {
            get => _selectedDayTotalCalories;
            set => Set(ref _selectedDayTotalCalories, value);
        }

        private string _newMealPlanTitle;
        public string NewMealPlanTitle
        {
            get => _newMealPlanTitle;
            set => Set(ref _newMealPlanTitle, value);
        }

        private ObservableCollection<Meal> _meals;
        public ObservableCollection<Meal> Meals
        {
            get => _meals;
            set => Set(ref _meals, value);
        }

        private ObservableCollection<Member> _members;
        public ObservableCollection<Member> Members
        {
            get => _members;
            set => Set(ref _members, value);
        }


        // Commands
        public ICommand LoadMealPlansCommand { get; }
        public ICommand AddMealCommand { get; }
        public ICommand AssignMealPlanCommand { get; }
        public ICommand LoadTrainerMealPlansCommand { get; }
        public ICommand LoadMemberMealPlansCommand { get; }
        public ICommand LoadMealsForMealPlanCommand { get; }
        public ICommand SelectDayCommand { get; }

        public MealPlanViewModel(IUnitOfWork unitOfWork, UserViewModel userViewModel)
        {
            _unitOfWork = unitOfWork;
            _userViewModel = userViewModel;

            // Commands
            LoadMealPlansCommand = new RelayCommand(async () => await LoadMealPlansAsync());
            AddMealCommand = new RelayCommand(async () => await AddMealAsync());
            AssignMealPlanCommand = new RelayCommand(async () => await AssignMealPlanAsync());
            LoadTrainerMealPlansCommand = new RelayCommand(async () => await LoadTrainerMealPlansAsync());
            LoadMemberMealPlansCommand = new RelayCommand(async () => await LoadMemberMealPlansAsync());
            LoadMealsForMealPlanCommand = new RelayCommand<int>(async (mealPlanId) => await LoadMealsForMealPlanAsync(mealPlanId));
            SelectDayCommand = new RelayCommand<string>((day) =>
            {
                SelectedDayMeals = new ObservableCollection<Meal>();
                SelectedDayTotalCalories = "Loading...";
                SelectedDay = day;
            });

            Initialize();
        }

        private void Initialize()
        {
            if (IsMember)
            {
                MealPlans = new ObservableCollection<MealPlan>();
            }
            else if (IsTrainer)
            {
                Meals = new ObservableCollection<Meal>();
                Members = new ObservableCollection<Member>();
            }
        }

        private async Task LoadMealPlansAsync()
        {
            try
            {
                if (_userViewModel.IsTrainer)
                {
                    var trainerId = (_userViewModel.LoggedUser as Trainer).Id;
                    var mealPlans = await _unitOfWork.MealPlans.GetMealPlansByTrainerAsync(trainerId);
                    MealPlans = new ObservableCollection<MealPlan>(mealPlans);
                }
                else if (_userViewModel.IsMember)
                {
                    var memberId = (_userViewModel.LoggedUser as Member).Id;
                    var mealPlans = await _unitOfWork.MealPlans.GetMealPlansByMemberAsync(memberId);
                    MealPlans = new ObservableCollection<MealPlan>(mealPlans);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading meal plans: {ex.Message}");
            }
        }

        //private async Task LoadSelectedDayMealsAsync()
        //{
        //    try
        //    {
        //        var userId = _userViewModel.LoggedUser.Id;
        //        var mealPlan = await _unitOfWork.MealPlans.GetMealPlanByDayAsync(SelectedDay, userId);

        //        if (mealPlan != null && mealPlan.Meals.Any())
        //        {
        //            SelectedDayMeals = new ObservableCollection<Meal>(mealPlan.Meals);
        //            SelectedDayTotalCalories = $"Total Calories: {mealPlan.Meals.Sum(m => m.TotalCalories)} kcal";
        //        }
        //        else
        //        {
        //            SelectedDayMeals = new ObservableCollection<Meal>();
        //            SelectedDayTotalCalories = "No meals available for this day.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Error loading meals for {SelectedDay}: {ex.Message}");
        //        SelectedDayTotalCalories = "Error loading data.";
        //    }
        //}

        public async Task LoadSelectedDayMealsAsync()
        {
            try
            {
                int userId;
                if (_userViewModel.LoggedUser is Member member)
                {
                    userId = member.Id; // Access the Id of the logged-in member
                }
                else if (_userViewModel.LoggedUser is Trainer trainer)
                {
                    userId = trainer.Id; // Access the Id of the logged-in trainer
                }
                else
                {
                    throw new InvalidOperationException("LoggedUser is neither Member nor Trainer.");
                }

                var mealPlan = await _unitOfWork.MealPlans.GetMealPlanByDayAsync(SelectedDay, userId);
                if (mealPlan != null && mealPlan.Meals.Any())
                {
                    SelectedDayMeals = new ObservableCollection<Meal>(mealPlan.Meals);
                    SelectedDayTotalCalories = $"Total Calories: {mealPlan.Meals.Sum(m => m.TotalCalories)} kcal";
                }
                else
                {
                    SelectedDayMeals = new ObservableCollection<Meal>();
                    SelectedDayTotalCalories = "No meals available for this day.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading meals for {SelectedDay}: {ex.Message}");
                SelectedDayTotalCalories = "Error loading data.";
            }
        }

        private async Task LoadTrainerMealPlansAsync()
        {
            try
            {
                if (IsTrainer)
                {
                    var trainerId = (_userViewModel.LoggedUser as Trainer).Id;
                    var mealPlans = await _unitOfWork.MealPlans.GetMealPlansByTrainerAsync(trainerId);

                    MealPlans = new ObservableCollection<MealPlan>(mealPlans);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading meal plans for trainer: {ex.Message}");
            }
        }

        private async Task LoadMemberMealPlansAsync()
        {
            try
            {
                if (IsMember)
                {
                    var memberId = (_userViewModel.LoggedUser as Member).Id;
                    var mealPlans = await _unitOfWork.MealPlans.GetMealPlansByMemberAsync(memberId);

                    MealPlans = new ObservableCollection<MealPlan>(mealPlans);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading meal plans for member: {ex.Message}");
            }
        }

        private async Task LoadMealsForMealPlanAsync(int mealPlanId)
        {
            try
            {
                var mealPlan = await _unitOfWork.MealPlans.GetByIdWithDetailsAsync(mealPlanId);

                if (mealPlan != null)
                {
                    SelectedDayMeals = new ObservableCollection<Meal>(mealPlan.Meals);
                    SelectedDayTotalCalories = $"Total Calories: {mealPlan.Meals.Sum(m => m.TotalCalories)} kcal";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading meals for meal plan: {ex.Message}");
            }
        }

        private async Task AddMealAsync()
        {
            if (IsTrainer && !string.IsNullOrWhiteSpace(NewMealPlanTitle))
            {
                var trainer = _userViewModel.LoggedUser as Trainer;
                var meal = new Meal(NewMealPlanTitle, MealType.Breakfast, 0); // Example for Breakfast
                Meals.Add(meal);
            }
        }

        private async Task AssignMealPlanAsync()
        {
            if (IsTrainer && SelectedMember != null)
            {
                var trainer = _userViewModel.LoggedUser as Trainer;
                var mealPlan = new MealPlan(NewMealPlanTitle, trainer);

                foreach (var meal in Meals)
                {
                    mealPlan.AddMeal(meal);
                }

                mealPlan.AssignToMember(SelectedMember);
                _unitOfWork.MealPlans.AddMealPlanAsync(mealPlan);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
