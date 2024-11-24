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

        public string DbPath { get; }

        public GymManagementDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Combine(path, "GymManagementSystem.db");
        }

        // Constructor for dependency injection and testing
        public GymManagementDbContext(DbContextOptions<GymManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

        // Apply Fluent API configurations (to establish relationships between the entities)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Admin
            modelBuilder.Entity<Admin>()
                .Property(a => a.Username)
                .HasMaxLength(45);

            modelBuilder.Entity<Admin>()
                .Property(a => a.Password)
                .HasMaxLength(45);

            modelBuilder.Entity<Admin>()
                .HasMany(a => a.Notifications)
                .WithOne(n => n.Admin)
                .HasForeignKey(n => n.AdminId)
                .IsRequired(false);

            //Booking
            modelBuilder.Entity<Booking>
                .HasOne(b => b.Class)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClassId);

            modelBuilder.Entity<Bookings>
                .HasOne(b => b.Member)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MemberId)
                .IsRequired(false);

            //Class
            modelBuilder.Entity<Class>
                .HasOne(c => c.Trainer)
                .WithMany(t => t.Classes)
                .HasForeignKey(c => c.TrainerId);

            modelBuilder.Entity<Class>
                .HasMany(c => c.Review)
                .WithOne(r => r.Class)
                .HasForeignKey(r => r.ReviewId)
                .IsRequired(false);

            //Meal
            modelBuilder.Entity<Meal>
                .HasMany(m => m.MealIngredients)
                .WithOne(mi => mi.Meal)
                .HasForeignKey(mi => mi.MealId);

            modelBuilder.Entity<Meal>
                .HasOne(m => m.MealPlan)
                .WithMany(mp => mp.Meals)
                .HasForeignKey(m => m.MealPlanId);

            //MealPlan
            modelBuilder.Entity<MealPlan>
                .HasOne(mp => mp.Trainer)
                .WithMany(t => t.MealPlans)
                .HasForeignKey(mp=> mp.TrainerId)
                .IsRequired(false);

            modelBuilder.Entity<MealPlan>
                .HasOne(mp => mp.Member)
                .WithMany(mb => mb.MealPlans)
                .HasForeignKey(mp => mp.MemberId)
                .IsRequired(false);

        }


    }
}
