using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    public class Admin : Entity
    {
        // Properties
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }

        // Navigation Properties
        public ICollection<Notification> Notifications { get; private set; }

        // Constructors
        private Admin()
        {
            // For EF Core
            Notifications = new HashSet<Notification>();
        }

        public Admin(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required.", nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required.", nameof(password));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));

            Email = email;
            Username = username;
            Password = password; // Hashing should happen in the application layer.

            Notifications = new HashSet<Notification>();
        }

        // Methods
        public void ChangePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword)) throw new ArgumentException("Password cannot be empty.", nameof(newPassword));
            Password = newPassword; // password hashing happens in the application layer.
        }

        public void UpdateUsername(string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername)) throw new ArgumentException("Username cannot be empty.", nameof(newUsername));
            Username = newUsername;
        }
    }
}
