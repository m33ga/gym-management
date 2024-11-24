using System;
using System.Collections.Generic;
using System.Text;
using GymManagement.Domain.Models;
using GymManagement.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure
{
    public class GymManagementDbContext: DbContext
    {
        public DbSet<Admin>Admins { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealIngredient> MealIngredients { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Trainer> Trainers { get; set; }

    }
}
