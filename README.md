# Gym Management App

## Disclaimer (for part II of project)
- **This is a development environment! The console app is just a way to test some functionalities, it does not represent in a total way the userflow. Many exceptions are thrown and are not handled since this will be handled in presentation layer. Some data is hardcoded for now to make testing easier :)**

#### To make it easier, we seeded some data in the database, therefore we have the following default data:

1. admin:
- email: admin@ex.com
- password: admin
2. members:
- email: alice@ex.com
- password: password3
- email: bob@ex.com
- password: password4
3. trainers:
- email: john@ex.com
- password: password1
- email: jane@ex.com
- password: password2
4. some classes(trainings) and memberships are also created to populate the database

#### Features that have been tested in console until now:
1. Create Users and other simple entities
2. Retrieve simple information
3. Search User by email
4. Hash user passwords
5. Update user details
6. Delete entities
7. Find Available classes
8. Find Classes booked by a member
9. Find All classes created by a Trainer
10. Find All classes created by a Trainer which have been booked by a User
11. Book a Class
12. Add a review to a class
13. Find rating of a Trainer
14. Add a notification for a user
15. Mark notification as read



## Overview

The Gym Management App is a Universal Windows Platform (UWP) application developed in C# to streamline gym operations and provide a seamless experience for members and trainers. Members can book workout sessions, receive customized meal plans, and track their training history. Trainers manage their availability and create tailored meal plans based on each member’s fitness goals. The app includes a range of functionalities, such as notifications, reviews, and leaderboard tracking for trainers.

## Project Goals and Objectives

### Project Goals
1. **Simplify Workout Session Booking**: Enable members to view trainer availability, book sessions, and receive reminders.
2. **Enhance Trainer-Member Interaction**: Facilitate feedback through reviews and tools for trainers to create customized meal plans.
3. **Personalized Meal Planning**: Trainers can adapt meal plans to members' goals, downloadable in PDF format.
4. **Admin and Membership Management**: Admins manage profiles and memberships, tracking workout limits.
5. **Implement Notifications**: Notify members of bookings, cancellations, reminders, and meal plan updates.

### Objectives
1. **Member Functionalities**: Booking, reviewing, accessing personalized meal plans, and viewing membership details.
2. **Trainer Tools**: Session scheduling and personalized meal plan creation.
3. **Admin Controls**: Manage member and trainer profiles, oversee notifications.
4. **Notifications**: Trigger alerts on key events like session bookings and reminders.

## Technical Details

### Project Structure

This project adheres to a Clean Architecture approach with separate layers for Presentation, Domain Model, and Infrastructure, implemented as separate Visual Studio projects. It uses the Model-View-ViewModel (MVVM) pattern for effective data binding and a structured codebase.

#### Key Components
- **Presentation Layer**: Handles UI and user interactions, built with UWP XAML.
- **Domain Model Layer**: Encapsulates business logic, ensuring that the core functionality remains consistent.
- **Infrastructure Layer**: Manages data access using Entity Framework Core with SQLite for data persistence.

### Database Structure

Entity Framework Core is used to map classes to database tables, providing a relational model. Key entities include:
- **Member**: Stores personal information, goals, and remaining workout sessions.
- **Trainer**: Manages availability and creates meal plans.
- **WorkoutSession**: Tracks session details such as date, time, and trainer.
- **Review**: Allows members to rate trainers after sessions.
- **MealPlan**: Customizable by trainers to suit member goals.
- **Notification**: Alerts members and trainers of relevant events.


### Development Tools and Libraries

- **Visual Studio 2022**: Primary development environment.
- **SQLite**: Database engine for data persistence.
- **Entity Framework Core**: ORM for database management.
- **Windows Community Toolkit**: Enhances UWP functionality.
- **WinUI**: Custom styling and controls.
- **MVVM**: Ensures structured data-binding and interaction.
- **Git/GitLab**: Version control and collaborative development.


## Authors

- [@Liubomyr Kravchuk](https://gitlab.estig.ipb.pt/a61511)
- [@Mihai Gurduza](https://gitlab.estig.ipb.pt/m320964)
- [@Patricia Moraru](https://gitlab.estig.ipb.pt/m320965)
- [@Vladyslav Tkachuk](https://gitlab.estig.ipb.pt/m320963)
