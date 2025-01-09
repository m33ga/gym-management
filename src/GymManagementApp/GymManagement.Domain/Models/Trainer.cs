using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    public class Trainer : Entity
    {
        // Properties
        public string FullName { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public string Username { get; private set; }
        public string PhoneNumber { get; private set; }
        public byte[] Image { get; private set; } // profile picture
        public string Bio { get; private set; } // short bio}

        // Navigation Properties
        public ICollection<Class> Classes { get; private set; }
        public ICollection<MealPlan> MealPlans { get; private set; }
        public ICollection<Notification> Notifications { get; private set; }
        public ICollection<Review> Reviews { get; private set; }

        // Constructors
        private Trainer()
        {
            // For EF Core
            Classes = new HashSet<Class>();
            MealPlans = new HashSet<MealPlan>();
            Notifications = new HashSet<Notification>();
            Reviews = new HashSet<Review>();
        }

        public Trainer(string fullName, string password, string email, string username, string phoneNumber, byte[] image = null)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full name is required.", nameof(fullName));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required.", nameof(password));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required.", nameof(username));
            if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentException("Phone number is required.", nameof(phoneNumber));
            

            FullName = fullName;
            Password = password; // Hashing should happen in the application layer.
            Email = email;
            Username = username;
            PhoneNumber = phoneNumber;
            Image = image;

            Classes = new HashSet<Class>();
            MealPlans = new HashSet<MealPlan>();
            Notifications = new HashSet<Notification>();
            Reviews = new HashSet<Review>();
        }

        // Methods
        public void UpdateProfile(string fullName, string phoneNumber, byte[] image = null)
        {
            if (!string.IsNullOrWhiteSpace(fullName))
                FullName = fullName;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                PhoneNumber = phoneNumber;

            if (image != null)
                Image = image;
        }

        public void ChangePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword)) throw new ArgumentException("Password cannot be empty.", nameof(newPassword));
            Password = newPassword; // Hashing should be handled before calling this method.
        }


        /// <summary>
        /// Schedules a new class for the trainer.
        /// </summary>
        /// <param name="newClass">The class to be scheduled.</param>
        public void ScheduleClass(Class newClass)
        {
            if (newClass == null) throw new ArgumentNullException(nameof(newClass), "Class cannot be null.");

            // no overlap with any existing classes
            foreach (var scheduledClass in Classes)
            {
                if (scheduledClass.ScheduledDate < newClass.ScheduledDate.AddMinutes(newClass.DurationInMinutes) &&
                    newClass.ScheduledDate < scheduledClass.ScheduledDate.AddMinutes(scheduledClass.DurationInMinutes))
                {
                    throw new InvalidOperationException("The new class overlaps with an existing scheduled class.");
                }
            }

            Classes.Add(newClass);
        }
    }
}
