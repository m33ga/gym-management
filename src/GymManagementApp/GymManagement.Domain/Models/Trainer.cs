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
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Image { get; set; } // profile picture

        // Navigation Properties
        public ICollection<Class> Classes { get; set; }
        public ICollection<MealPlan> MealPlans { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Review> Reviews { get; set; }

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

        public string ImageBase64
        {
            get
            {
                return Image != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(Image)}" : null;
            }
        }
    }
}
