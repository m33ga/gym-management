﻿using GymManagement.Domain.Models;
using GymManagement.Domain.ViewModel;
using GymManagement.Infrastructure;
using GymManagement.Infrastructure.Repository;
using GymManagement.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GymManagement.UWP.Views.Dashboard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MemberDashboardPage : Page
    {
        public DashboardViewModel ViewModel { get; }

        public MemberDashboardPage()
        {
            this.InitializeComponent();
            //UpcomingWorkoutsList.SelectedIndex = -1;
            //PastWorkoutsList.SelectedIndex = -1;
            var dbContext = new GymManagementDbContext();
            var userViewModel = App.UserViewModel;
            ViewModel = new DashboardViewModel(new ClassRepository(dbContext), new BookingRepository(dbContext), userViewModel, new ReviewRepository(dbContext));

            this.DataContext = ViewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadWorkoutsAsync();
        }

        private async void PastWorkoutsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedWorkout = e.AddedItems[0] as Class;
                await ViewModel.LoadWorkoutDetailsAsync(selectedWorkout);
                DefaultMessage.Visibility = Visibility.Collapsed;
                WorkoutDetailsPanel.Visibility = Visibility.Visible;
                RatingPanel.Visibility = Visibility.Visible;
            }
        }

        private async void UpcomingWorkoutsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedWorkout = e.AddedItems[0] as Class;
                await ViewModel.LoadWorkoutDetailsAsync(selectedWorkout);
                DefaultMessage.Visibility = Visibility.Collapsed;
                WorkoutDetailsPanel.Visibility = Visibility.Visible;
                RatingPanel.Visibility = Visibility.Collapsed;
            }

        }

        private async void RatingControl_OnValueChanged(RatingControl sender, object args)
        {
            await ViewModel.SaveRatingAsync();
        }
    }

}
