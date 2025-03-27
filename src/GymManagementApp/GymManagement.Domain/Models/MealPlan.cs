using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    public class MealPlan : Entity
    {
        // Properties
        public string Title { get; private set; } // E.g., "Weight Loss Plan - Week 1"
        public int TotalCalories { get; private set; } // Total calories for the entire plan
        public int TrainerId { get; private set; }
        public Trainer Trainer { get; private set; }
        public int? MemberId { get; private set; }
        public Member Member { get; private set; }
        public ICollection<Meal> Meals { get; private set; }

        // Constructors
        private MealPlan()
        {
            // For EF Core
            Meals = new List<Meal>();
        }

        public MealPlan(string title, Trainer trainer)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));
            if (trainer == null) throw new ArgumentNullException(nameof(trainer), "Trainer is required.");

            Title = title;
            Trainer = trainer;
            TrainerId = trainer.Id;
            Meals = new List<Meal>();
            TotalCalories = 0;
        }

        // Methods
        public void AssignToMember(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member), "Member cannot be null.");
            Member = member;
            MemberId = member.Id;
        }

        public void AddMeal(Meal meal)
        {
            if (meal == null) throw new ArgumentNullException(nameof(meal), "Meal cannot be null.");
            Meals.Add(meal);
            TotalCalories += meal.TotalCalories;
        }
    }
}
