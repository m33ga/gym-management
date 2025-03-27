using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    public class Class : Entity
    {
        // Properties
        public string Name { get;   set; }
        public string Description { get;   set; }
        public DateTime ScheduledDate { get;   set; } // Date and time the class is scheduled
        public int DurationInMinutes { get;   set; } // Duration of the class in minutes
        public int TrainerId { get;   set; }
        public Trainer Trainer { get; private set; } // Navigation property for the Trainer
        public bool IsAvailable { get; set; }
        public int? MemberId { get; private set; } // Member who booked the class (if any) nullable
        public Member Member { get; private  set; } // Navigation property for the Member
        public Review Review { get; private  set; }
        public Booking Booking { get; private set; }

        public string FormattedTrainerWorkoutDetails
        {
            get
            {
                return $"{Name} class on {ScheduledDate:MM.dd.yyyy} at {ScheduledDate:HH:mm} with member {Member?.FullName}";
            }
        }

        public string FormattedMemberWorkoutDetails
        {
            get
            {
                return $"{Name} class on {ScheduledDate:MM.dd.yyyy} at {ScheduledDate:HH:mm} with trainer {Trainer?.FullName}";
            }
        }


        // Constructors
        private Class()
        {
            // For EF Core
        }

        public Class(string name, string description, DateTime scheduledDate, int durationInMinutes, Trainer trainer)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required.", nameof(description));
            if (durationInMinutes <= 0) throw new ArgumentOutOfRangeException(nameof(durationInMinutes), "Duration must be positive.");
            if (scheduledDate < DateTime.UtcNow) throw new ArgumentOutOfRangeException(nameof(scheduledDate), "Scheduled date cannot be in the past.");
            if (trainer == null) throw new ArgumentNullException(nameof(trainer), "Trainer is required.");

            Name = name;
            Description = description;
            ScheduledDate = scheduledDate;
            DurationInMinutes = durationInMinutes;
            Trainer = trainer ?? throw new ArgumentNullException(nameof(trainer), "Trainer is required.");
            TrainerId = trainer.Id;
            IsAvailable = true; // Default to available when created
            trainer.ScheduleClass(this);
        }

        // Methods
        public void BookClass(int? memberId)
        {
            if (!IsAvailable)
                throw new InvalidOperationException("Class is already booked.");

            // Assign the member ID and mark the class as unavailable
            MemberId = memberId ?? throw new InvalidOperationException("Invalid member ID. Booking cannot proceed.");
            IsAvailable = false;
        }
        public void CancelBooking()
        {
            if (IsAvailable) throw new InvalidOperationException("No booking to cancel.");

            Member = null;
            MemberId = null;
            IsAvailable = true;
        }

        public bool IsScheduledFor(DateTime date)
        {
            return ScheduledDate.Date == date.Date;
        }
    }
}
