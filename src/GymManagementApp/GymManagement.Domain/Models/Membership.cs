using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    /// <summary>
    /// Membership class to handle membership details.
    /// </summary>
    public class Membership : Entity
    {
        // Properties
        public string Tier { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public int TrainingsQuantity { get; private set; }
        public int Duration { get; private set; } // Duration in days

        // Navigation Properties
        public ICollection<Member> Members { get; private set; }

        // Constructors
        private Membership()
        {
            // For EF Core (required for database migrations)
            Members = new HashSet<Member>();
        }

        public Membership(string tier, decimal price, string description, int trainingsQuantity, int duration)
        {
            if (string.IsNullOrWhiteSpace(tier)) throw new ArgumentException("Tier cannot be empty.", nameof(tier));
            if (price <= 0) throw new ArgumentException("Price must be greater than zero.", nameof(price));
            if (trainingsQuantity <= 0) throw new ArgumentException("TrainingsQuantity must be greater than zero.", nameof(trainingsQuantity));
            if (duration <= 0) throw new ArgumentException("Duration must be greater than zero.", nameof(duration));

            Tier = tier;
            Price = price;
            Description = description;
            TrainingsQuantity = trainingsQuantity;
            Duration = duration;

            Members = new HashSet<Member>();
        }

        // Methods
        public void UpdateMembershipDetails(string tier, decimal price, string description, int trainingsQuantity, int duration)
        {
            if (!string.IsNullOrWhiteSpace(tier))
                Tier = tier;

            if (price > 0)
                Price = price;

            Description = description ?? Description;

            if (trainingsQuantity > 0)
                TrainingsQuantity = trainingsQuantity;

            if (duration > 0)
                Duration = duration;
        }
    }
}
