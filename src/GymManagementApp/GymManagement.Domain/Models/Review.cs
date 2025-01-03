using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    public class Review : Entity
    {
        // Properties
        public int ClassId { get; private set; }
        public Class Class { get; private set; }

        public int MemberId { get; private set; }
        public Member Member { get; private set; }

        public int Rating { get; private set; }
        //public string Comment { get; private set; }
        public DateTime DateSubmitted { get; set; }
        public Trainer Trainer { get; private set; }
        public int TrainerId { get; set; }

        // Constructor
        // TODO: in application layer,
        // TODO:    add logic for review only after class has ended and
        // TODO:    prevent multiple reviews from the same member for the same class.
        public Review(int memberId, int classId, int rating, string comment, int trainerId)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

            MemberId = memberId;
            ClassId = classId;
            Rating = rating;
            Comment = comment;
            DateSubmitted = DateTime.UtcNow;
            TrainerId = trainerId;
        }

        // Methods
        public void UpdateReview(int rating, string comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

            Rating = rating;
            Comment = comment;
            DateSubmitted = DateTime.UtcNow; // Update submission date
        }
    }
}
