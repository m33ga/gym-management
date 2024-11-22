using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.Domain.SeedWork
{
    /// <summary>
    /// Purpose: Represents and validates email addresses.
    /// Usage: Use in Member and Trainer entities.
    /// </summary>
    public class Email : ValueObject
    {
        public string Address { get; private set; }

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address) || !address.Contains("@"))
                throw new ArgumentException("Invalid email address.");

            Address = address;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }
    }
}
