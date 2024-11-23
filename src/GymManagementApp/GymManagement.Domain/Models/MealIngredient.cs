using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    public class MealIngredient : Entity
    {
        // Properties
        public string Name { get; private set; } // Ingredient name, e.g., "Chicken Breast"
        public int QuantityInGrams { get; private set; } // Quantity in grams
        public int Calories { get; private set; } // Total calories for this ingredient

        // Constructors
        private MealIngredient()
        {
            // For EF Core
        }

        public MealIngredient(string name, int quantityInGrams, int calories)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
            if (quantityInGrams <= 0) throw new ArgumentOutOfRangeException(nameof(quantityInGrams), "Quantity must be greater than 0.");
            if (calories < 0) throw new ArgumentOutOfRangeException(nameof(calories), "Calories cannot be negative.");

            Name = name;
            QuantityInGrams = quantityInGrams;
            Calories = calories;
        }
    }
}
