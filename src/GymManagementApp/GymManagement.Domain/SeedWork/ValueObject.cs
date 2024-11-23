using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GymManagement.Domain.SeedWork
{
    /// <summary>
    /// Purpose: Base class for value objects, which are immutable and compared by value.
    /// Usage: Extend this for reusable, immutable concepts (e.g., Email, Calories, Goal).
    /// </summary>
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                    CombineHashCodes(current, obj?.GetHashCode() ?? 0));
        }

        private static int CombineHashCodes(int h1, int h2)
        {
            unchecked
            {
                return ((h1 << 5) + h1) ^ h2;
            }
        }
    }
}