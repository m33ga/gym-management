using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.Domain.SeedWork
{
    /// <summary>
    /// Purpose: Serves as the base class for all entities. Provides a unique identifier (Id) and common equality logic.
    /// Usage: Extend this in all entity classes (e.g., Member, Trainer, Booking).
    /// </summary>
    public abstract class Entity
    {
        public int Id { get; protected set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (Entity)obj;
            return Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
