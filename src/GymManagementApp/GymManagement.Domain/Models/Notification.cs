using System;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    public class Notification : Entity
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } 

        public int? AdminId { get; private set; }
        public Admin Admin { get; private set; }

        public int? MemberId { get; private set; }
        public Member Member { get; private set; }

        public int? TrainerId { get; private set; }
        public Trainer Trainer { get; private set; }

        private Notification() { } // For EF Core

        public Notification(string text, DateTime date, string status, int? adminId, int? memberId = null, int? trainerId = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Notification text is required.", nameof(text));

            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Notification status is required.", nameof(status));

            Text = text;
            Date = date;
            Status = status;

            AdminId = adminId;
            MemberId = memberId;
            TrainerId = trainerId;
        }

        public void MarkAsRead()
        {
            if (Status == "Read")
                throw new InvalidOperationException("Notification is already marked as read.");

            Status = "Read";
        }

        public void UpdateText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
                throw new ArgumentException("Notification text cannot be empty.", nameof(newText));

            Text = newText;
        }
    }
}

