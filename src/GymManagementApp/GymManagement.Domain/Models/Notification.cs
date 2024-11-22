using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    public class Notification : Entity
    {
        // Properties
        public string Text { get; private set; }
        public DateTime Date { get; private set; }
        public string Status { get; private set; } //  "Unread", "Read"

        // Relationships
        public int? AdminId { get; private set; }
        public Admin Admin { get; private set; }

        public int? MemberId { get; private set; }
        public Member Member { get; private set; }

        public int? TrainerId { get; private set; }
        public Trainer Trainer { get; private set; }

        // Constructors
        private Notification()
        {
            // For EF Core
        }

        public Notification(string text, DateTime date, string status, Admin admin = null, Member member = null, Trainer trainer = null)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Notification text is required.", nameof(text));
            if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Notification status is required.", nameof(status));

            Text = text;
            Date = date;
            Status = status;

            Admin = admin;
            Member = member;
            Trainer = trainer;
        }

        // Methods
        public void MarkAsRead()
        {
            Status = "Read";
        }

        public void UpdateText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText)) throw new ArgumentException("Notification text cannot be empty.", nameof(newText));
            Text = newText;
        }
    }
}
