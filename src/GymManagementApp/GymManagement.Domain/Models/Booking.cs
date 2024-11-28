using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Domain.Enums;
using GymManagement.Domain.SeedWork;

namespace GymManagement.Domain.Models
{
    public class Booking : Entity
    {
        // Properties
        public int MemberId { get; private set; }
        public Member Member { get; private set; }

        public int ClassId { get; private set; }
        public Class Class { get; private set; }

        public DateTime BookingDate { get; private set; }
        public BookingStatus Status { get; private set; }

        // Constructor
        public Booking(int memberId, int classId, DateTime bookingDate)
        {
            MemberId = memberId;
            ClassId = classId;
            BookingDate = bookingDate;

            Status = BookingStatus.Not_Booked; // Default status
        }

        // Methods
        public void Confirmed()
        {
            if(Status == BookingStatus.Confirmed) 
                throw new InvalidOperationException("Booking is already confirmed");

            Status = BookingStatus.Confirmed;
        }
        public void Cancel()
        {
            if (Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Booking is already cancelled.");

            Status = BookingStatus.Cancelled;
        }
    }
}
