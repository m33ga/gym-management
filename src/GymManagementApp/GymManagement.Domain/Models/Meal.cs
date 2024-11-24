using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Domain.Enums;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    public class Meal : Entity
    {
        // Properties
        public string Name { get; private set; } // E.g., "Breakfast - Monday"
        public MealType Type { get; private set; } // Enum: Breakfast, Lunch, Dinner
        public int DayOfWeek { get; private set; } // 0 = Sunday, 1 = Monday, etc.
        public int TotalCalories { get; private set; } // Total calories for this meal
        public ICollection<MealIngredient> Ingredients { get; private set; }
        public MealPlan MealPlan { get; private set; }
        public int MealPlanId { get; private set; }


        // Constructors
        private Meal()
        {
            // For EF Core
            Ingredients = new List<MealIngredient>();
        }

        public Meal(string name, MealType type, int dayOfWeek)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
            if (dayOfWeek < 0 || dayOfWeek > 6) throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "Day of the week must be between 0 (Sunday) and 6 (Saturday).");

            Name = name;
            Type = type;
            DayOfWeek = dayOfWeek;
            Ingredients = new List<MealIngredient>();
            TotalCalories = 0;
        }

        // Methods
        public void AddIngredient(MealIngredient ingredient)
        {
            if (ingredient == null) throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null.");
            Ingredients.Add(ingredient);
            TotalCalories += ingredient.Calories;
        }
    }
}
