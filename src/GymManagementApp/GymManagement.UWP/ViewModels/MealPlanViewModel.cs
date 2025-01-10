using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GymManagement.Domain.Enums;
using GymManagement.Domain;
using GymManagement.Domain.Models;
using GymManagement.Domain.Repository;
using GymManagement.UWP.Helpers;

namespace GymManagement.UWP.ViewModels
{
    public class MealPlanViewModel : BindableBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserViewModel _userViewModel;

        // Role identification
        public bool IsMember => _userViewModel.LoggedUser is Member;
        public bool IsTrainer => _userViewModel.LoggedUser is Trainer;

        // Data for Members
        private ObservableCollection<MealPlan> _mealPlans;
        public ObservableCollection<MealPlan> MealPlans
        {
            get => _mealPlans;
            set => Set(ref _mealPlans, value);
        }

        // Data for Trainers
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

        private Member _selectedMember;
        public Member SelectedMember
        {
            get => _selectedMember;
            set => Set(ref _selectedMember, value);
        }

        // Commands
        public ICommand LoadMealPlansCommand { get; }
        public ICommand AddMealCommand { get; }
        public ICommand AssignMealPlanCommand { get; }

        public MealPlanViewModel(IUnitOfWork unitOfWork, UserViewModel userViewModel)
        {
            _unitOfWork = unitOfWork;
            _userViewModel = userViewModel;

            LoadMealPlansCommand = new RelayCommand(async () => await LoadMealPlansAsync());
            AddMealCommand = new RelayCommand(async () => await AddMealAsync());
            AssignMealPlanCommand = new RelayCommand(async () => await AssignMealPlanAsync());

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
            if (IsMember)
            {
                var member = _userViewModel.LoggedUser as Member;
                var plans = await _unitOfWork.MealPlans.GetMealPlansByMemberAsync(member.Id);
                MealPlans = new ObservableCollection<MealPlan>(plans);
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
