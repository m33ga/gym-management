﻿using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    public class Member : Entity
    {
        // Properties
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public int RemainingWorkouts { get; set; }
        public byte[] Image { get; set; } // For profile picture

        // Relationships
        public int MembershipId { get; set; }
        public Membership Membership { get; set; }

        public ICollection<Notification> Notifications { get; private set; }
        public ICollection<Review> Reviews { get; private set; }
        public ICollection<MealPlan> MealPlans { get; private set; }
        public ICollection<Booking> Bookings { get; private set; }

        // Constructors
        private Member()
        {
            // For EF Core
            Notifications = new List<Notification>();
            Reviews = new List<Review>();
            MealPlans = new List<MealPlan>();
            Bookings = new List<Booking>();
        }

        public Member(
            string fullName,
            string email,
            string password,
            string username,
            string phoneNumber,
            float weight,
            float height,
            int remainingWorkouts,
            Membership membership,
            byte[] image = null)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full name is required.", nameof(fullName));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required.", nameof(password));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required.", nameof(username));
            if (membership == null) throw new ArgumentNullException(nameof(membership), "Membership is required.");

            FullName = fullName;
            Email = email;
            Password = password;
            Username = username;
            PhoneNumber = phoneNumber;
            Weight = weight;
            Height = height;
            RemainingWorkouts = remainingWorkouts;
            Membership = membership;
            MembershipId = membership.Id;
            Image = image;

            Notifications = new List<Notification>();
            Reviews = new List<Review>();
            MealPlans = new List<MealPlan>();
            Bookings = new List<Booking>();
        }

        // Methods
        public void UpdateProfile(string fullName, string phoneNumber, float weight, float height, byte[] image = null)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full name cannot be empty.", nameof(fullName));
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Weight = weight;
            Height = height;
            Image = image;
        }
        public string ImageBase64
        {
            get
            {
                return Image != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(Image)}" : null;
            }
        }

        public void AddNotification(Notification notification)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification), "Notification cannot be null.");
            Notifications.Add(notification);
        }

        public void AddMealPlan(MealPlan mealPlan)
        {
            if (mealPlan == null) throw new ArgumentNullException(nameof(mealPlan), "Meal plan cannot be null.");
            MealPlans.Add(mealPlan);
        }

        public void UpdatePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Password cannot be empty.", nameof(newPassword));
            if (newPassword.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters long.", nameof(newPassword));

            Password = newPassword;
        }


        public void UseWorkout()
        {
            if (RemainingWorkouts <= 0) throw new InvalidOperationException("No remaining workouts available.");
            RemainingWorkouts--;
        }
    }
}
