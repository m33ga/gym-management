using GymManagement.Domain.Enums;
using GymManagement.Domain.Models;
using GymManagement.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GymManagement.UWP.Helpers;

namespace GymManagement.UWP.ViewModels
{
    public class MealPlanTrainerViewModel : BindableBase
    {
        private readonly MealPlanRepository _mealPlanRepository;
        private readonly MemberRepository _memberRepository;
        private readonly UserViewModel _userViewModel;


        public MealPlanTrainerViewModel(MealPlanRepository mealPlanRepository, MemberRepository memberRepository,
            UserViewModel userViewModel)
        {
            _mealPlanRepository = mealPlanRepository;
            _memberRepository = memberRepository;
            _userViewModel = userViewModel;


            LoadMembers();
            LoadDays();
            LoadMealTypes();
            Dishes = new ObservableCollection<DishViewModel>();
            AddDishCommand = new RelayCommand(AddDish);
            AssignMealPlanCommand = new RelayCommand(AssignMealPlan);
        }

        public ObservableCollection<Member> Members { get; set; }
        public Member SelectedMember { get; set; }
        public ObservableCollection<int> Days { get; set; }
        public int SelectedDay { get; set; }
        public ObservableCollection<MealType> MealTypes { get; set; }
        public MealType SelectedMealType { get; set; }
        public string NewDishName { get; set; }
        public ObservableCollection<DishViewModel> Dishes { get; set; }
        public ICommand AddDishCommand { get; set; }
        public ICommand AssignMealPlanCommand { get; set; }

        private void LoadMembers()
        {
            Members = new ObservableCollection<Member>(_memberRepository.GetAllMembersAsync().Result);
        }

        private void LoadDays()
        {
            Days = new ObservableCollection<int>(Enumerable.Range(0, 7));
        }

        private void LoadMealTypes()
        {
            MealTypes = new ObservableCollection<MealType>(Enum.GetValues(typeof(MealType)).Cast<MealType>());
        }

        private void AddDish()
        {
            if (!string.IsNullOrWhiteSpace(NewDishName))
            {
                Dishes.Add(new DishViewModel { Name = NewDishName });
                NewDishName = string.Empty;
                OnPropertyChanged(nameof(NewDishName));
            }
        }

        private void AssignMealPlan()
        {
            // just don't throw exception :|

            //if (_userViewModel.LoggedUser is Trainer loggedTrainer)
            //{
            //    var mealPlan = new MealPlan("New Meal Plan", loggedTrainer);
            //    mealPlan.AssignToMember(SelectedMember);

            //    foreach (var dish in Dishes)
            //    {
            //        var meal = new Meal(dish.Name, SelectedMealType, SelectedDay);
            //        foreach (var ingredient in dish.Ingredients)
            //        {
            //            meal.AddIngredient(ingredient);
            //        }

            //        mealPlan.AddMeal(meal);
            //    }

            //    _mealPlanRepository.AddMealPlanAsync(mealPlan).Wait();
            //}
        }

        public class DishViewModel : BindableBase
        {
            public string Name { get; set; }

            public ObservableCollection<MealIngredient> Ingredients { get; set; } =
                new ObservableCollection<MealIngredient>();

            public string NewIngredientName { get; set; }
            public int NewIngredientQuantity { get; set; }
            public int NewIngredientCalories { get; set; }
            public ICommand AddIngredientCommand { get; set; }
            public ICommand RemoveIngredientCommand { get; set; }

            public DishViewModel()
            {
                AddIngredientCommand = new RelayCommand(AddIngredient);
                RemoveIngredientCommand = new RelayCommand<MealIngredient>(RemoveIngredient);
            }

            private void AddIngredient()
            {
                if (!string.IsNullOrWhiteSpace(NewIngredientName) && NewIngredientQuantity > 0 &&
                    NewIngredientCalories >= 0)
                {
                    Ingredients.Add(new MealIngredient(NewIngredientName, NewIngredientQuantity,
                        NewIngredientCalories));
                    NewIngredientName = string.Empty;
                    NewIngredientQuantity = 0;
                    NewIngredientCalories = 0;
                    OnPropertyChanged(nameof(NewIngredientName));
                    OnPropertyChanged(nameof(NewIngredientQuantity));
                    OnPropertyChanged(nameof(NewIngredientCalories));
                }
            }

            private void RemoveIngredient(MealIngredient ingredient)
            {
                Ingredients.Remove(ingredient);
            }
        }

    }
}
