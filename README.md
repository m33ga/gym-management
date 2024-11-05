# Gym Management App

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
