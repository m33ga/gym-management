﻿// <auto-generated />
using System;
using GymManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GymManagement.Infrastructure.Migrations
{
    [DbContext(typeof(GymManagementDbContext))]
    [Migration("20241124162632_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32");

            modelBuilder.Entity("GymManagement.Domain.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT")
                        .HasMaxLength(45);

                    b.Property<string>("Username")
                        .HasColumnType("TEXT")
                        .HasMaxLength(45);

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ClassId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MemberId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("MemberId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("DurationInMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MemberId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ScheduledDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("TrainerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("TrainerId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Meal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MealPlanId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalCalories")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MealPlanId");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.MealIngredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Calories")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MealId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("QuantityInGrams")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MealId");

                    b.ToTable("MealIngredients");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.MealPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MemberId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalCalories")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrainerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("TrainerId");

                    b.ToTable("MealPlans");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .HasColumnType("TEXT");

                    b.Property<float>("Height")
                        .HasColumnType("REAL");

                    b.Property<byte[]>("Image")
                        .HasColumnType("BLOB");

                    b.Property<int>("MembershipId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("RemainingWorkouts")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.Property<float>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("MembershipId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Membership", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tier")
                        .HasColumnType("TEXT");

                    b.Property<int>("TrainingsQuantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Memberships");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AdminId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int?>("MemberId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TrainerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("MemberId");

                    b.HasIndex("TrainerId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClassId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateSubmitted")
                        .HasColumnType("TEXT");

                    b.Property<int>("MemberId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrainerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("MemberId");

                    b.HasIndex("TrainerId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Trainer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Image")
                        .HasColumnType("BLOB");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Trainers");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Booking", b =>
                {
                    b.HasOne("GymManagement.Domain.Models.Class", "Class")
                        .WithMany("Bookings")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymManagement.Domain.Models.Member", "Member")
                        .WithMany("Bookings")
                        .HasForeignKey("MemberId");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Class", b =>
                {
                    b.HasOne("GymManagement.Domain.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId");

                    b.HasOne("GymManagement.Domain.Models.Trainer", "Trainer")
                        .WithMany("Classes")
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Meal", b =>
                {
                    b.HasOne("GymManagement.Domain.Models.MealPlan", "MealPlan")
                        .WithMany("Meals")
                        .HasForeignKey("MealPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GymManagement.Domain.Models.MealIngredient", b =>
                {
                    b.HasOne("GymManagement.Domain.Models.Meal", "Meal")
                        .WithMany("Ingredients")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GymManagement.Domain.Models.MealPlan", b =>
                {
                    b.HasOne("GymManagement.Domain.Models.Member", "Member")
                        .WithMany("MealPlans")
                        .HasForeignKey("MemberId");

                    b.HasOne("GymManagement.Domain.Models.Trainer", "Trainer")
                        .WithMany("MealPlans")
                        .HasForeignKey("TrainerId");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Member", b =>
                {
                    b.HasOne("GymManagement.Domain.Models.Membership", "Membership")
                        .WithMany("Members")
                        .HasForeignKey("MembershipId");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Notification", b =>
                {
                    b.HasOne("GymManagement.Domain.Models.Admin", "Admin")
                        .WithMany("Notifications")
                        .HasForeignKey("AdminId");

                    b.HasOne("GymManagement.Domain.Models.Member", "Member")
                        .WithMany("Notifications")
                        .HasForeignKey("MemberId");

                    b.HasOne("GymManagement.Domain.Models.Trainer", "Trainer")
                        .WithMany("Notifications")
                        .HasForeignKey("TrainerId");
                });

            modelBuilder.Entity("GymManagement.Domain.Models.Review", b =>
                {
                    b.HasOne("GymManagement.Domain.Models.Class", "Class")
                        .WithMany("Reviews")
                        .HasForeignKey("ClassId");

                    b.HasOne("GymManagement.Domain.Models.Member", "Member")
                        .WithMany("Reviews")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymManagement.Domain.Models.Trainer", null)
                        .WithMany("Reviews")
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
